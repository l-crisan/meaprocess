/*
 * Copyright (C) 2015 by Laurentiu-Gheorghe Crisan
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * As a special exception, you may use this file as part of a free
 * software library without restriction. Specifically, if other files
 * instantiate templates or use macros or inline functions from this
 * file, or you compile this file and link it with other files to
 * produce an executable, this file does not by itself cause the
 * resulting executable to be covered by the GNU General Public
 * License. This exception does not however invalidate any other
 * reasons why the executable file might be covered by the GNU Library
 * General Public License.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
#ifndef MPS_CORE_OBJECT_H
#define MPS_CORE_OBJECT_H

#include <vector>
#include <stdexcept>
#include <string>
#include <iostream>
#include <sstream>
#include <Pt/TypeTraits.h>
#include <Pt/Any.h>
#include <Pt/Types.h>

namespace mps{
namespace core{

#ifndef DOXYGEN_SHOULD_SKIP_THIS // Begin no doxygen documentation

class PropertyInfo
{
    public:
        PropertyInfo(const std::string& name)
        :_name(name)
        {}

        virtual ~PropertyInfo()
        {}

        virtual Pt::Any get() const = 0;

        virtual void set(const Pt::Any& value) = 0;

        const char* name() const
        { return _name.c_str(); }

    private:
        std::string _name;
};

template <typename A, typename C>
class ReadWriteProperty : public PropertyInfo
{
    public:
        ReadWriteProperty(const std::string& name, C& parent, A (C::*getter)() const, void (C::*setter)(A type) )
        : PropertyInfo(name)
        , _obj(&parent)
        , _getter( getter)
        , _setter(setter)
        {
        }

        Pt::Any get() const
        {
            return  (_obj->*_getter)();
        }

        void set(const Pt::Any& a)
        {
            typedef typename Pt::TypeTraits<A>::ConstReference ConstRefT ;

            ConstRefT val = Pt::any_cast<ConstRefT>(a) ;
            return  (_obj->*_setter)(val);
        }

    private:
        C* _obj;
        A (C::*_getter)() const;
        void (C::*_setter)(A);
};

#endif // End no doxygen documentation

/**@brief The base class for each loadable object from xml. */
class  Object
{
public:
    /**@brief Constructor
    *
    * @param objName The object name 
    */
    Object()
    { }

    /**@brief Destructor */
    virtual ~Object()
    { 
        for(Pt::uint32_t i = 0; i < _properties.size(); ++i)
            delete _properties[i];
    }

    /**@brief Called by the framework to append a child object to this object.
    *
    * Override this method to append child objects to this object.
    *
    * @param obj The child object.
    * @param type The type of the object.
    * @param subType The second type description.
    */
    virtual void addObject(Object* obj, const std::string& type, const std::string& subType)
    { }

    /**@brief Get a member property by name.
    *
    *
    * @param name The property name
    * @return The property value.
    */
    Pt::Any property(const std::string& name) const
    {
        std::vector<PropertyInfo*>::const_iterator it;
        for( it = _properties.begin(); it != _properties.end(); ++it)
        {
            if( name == (*it)->name())
                return (*it)->get();
        }

        throw std::logic_error(name);
    }

    /**@brief Set a member property.
    *
    *
    * @param name The property name.
    * @param name The property value.
    */
    void setProperty(const std::string& name, const Pt::Any& value)
    {
        std::vector<PropertyInfo*>::iterator it;
        for( it = _properties.begin(); it != _properties.end(); ++it)
        {
            if( name == (*it)->name())
            {
                try
                {
                    (*it)->set(value);
                    return;
                }
                catch(const std::bad_cast& ex) 
                                {
                                    std::stringstream ss;
                                    ss<<"mps::core::Object::setProperty() type mismatch for " << name<<" : "<<ex.what();

                                    throw std::logic_error(ss.str().c_str());
                }
            }
        }
    }

    /**@brief Register a property of the object
    *
    * The template parameter A and C are automaticaly detected by call.
    *
    * @param name The property name.
    * @param parent The object instance.
    * @param getter The property getter.
    * @param setter The property setter.
    */
    template <typename A, typename C>
    void registerProperty(const std::string& name, C& parent, A (C::*getter)() const, void (C::*setter)(A type))
    {
        _properties.push_back( new ReadWriteProperty<A, C>(name, parent, getter, setter) );
    }

private:
    std::vector<PropertyInfo*> _properties;

};

}}
#endif
