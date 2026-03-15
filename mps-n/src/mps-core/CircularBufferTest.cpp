//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
#undef PT_API_EXPORT
#include <Pt/Unit/TestMain.h>
#include <Pt/Unit/Assertion.h>
#include <Pt/Unit/TestSuite.h>
#include <Pt/Unit/RegisterTest.h>
#include <mps/CircularBuffer.h>
#include <vector>

class CircularBufferTest : public Pt::Unit::TestSuite
{
    public:
        CircularBufferTest()
        : Pt::Unit::TestSuite("CircularBufferTest")
        {
            Pt::Unit::TestSuite::registerMethod( "circularBuffer", *this, &CircularBufferTest::circularBuffer );
        }

    protected:
        void circularBuffer()
        {
            Pt::uint8_t data[5][5];

            for(size_t i = 0; i < 5; i++)
                for(size_t j = 0; j < 5; j++)
                    data[i][j] = j + 1;

            mps::CircularBuffer circularBuffer;

            circularBuffer.init(5,5);

            PT_UNIT_ASSERT( circularBuffer.noOfElements() == 0 );
            PT_UNIT_ASSERT( circularBuffer.elementSize() == 5);
            PT_UNIT_ASSERT( circularBuffer.isEmpty() );
            PT_UNIT_ASSERT( !circularBuffer.isFull() );

            circularBuffer.insert(&data[0][0]);
            circularBuffer.insert(&data[1][0]);
            const Pt::uint8_t* rec = circularBuffer.get();

            PT_UNIT_ASSERT( memcmp(&data[0],rec,5) == 0 );

            circularBuffer.next();

            PT_UNIT_ASSERT( circularBuffer.noOfElements() == 1 );

            circularBuffer.next();

            PT_UNIT_ASSERT( circularBuffer.isEmpty() );

            circularBuffer.reset();

            circularBuffer.init(5,5);

            PT_UNIT_ASSERT( circularBuffer.noOfElements() == 0 );
            PT_UNIT_ASSERT( circularBuffer.elementSize() == 5);
            PT_UNIT_ASSERT( circularBuffer.isEmpty() );
            PT_UNIT_ASSERT( !circularBuffer.isFull() );

            circularBuffer.insert(&data[0][0],3);
            bool overrun = false;
            try
            {
                circularBuffer.insert(&data[0][0],3);
            }
            catch (std::exception e)
            {
            	overrun = true;
            }

            PT_UNIT_ASSERT( overrun );

            circularBuffer.reset();
            circularBuffer.init(5,5);

            circularBuffer.insert(&data[0][0],2);
            circularBuffer.insert(&data[1][0],3);

            size_t max;
            circularBuffer.get(5,max);

            PT_UNIT_ASSERT( max == 5 );

            circularBuffer.next(3);

            circularBuffer.insert(&data[0][0],2);

            rec = circularBuffer.get(5, max);

            PT_UNIT_ASSERT( max == 4 );

            circularBuffer.next(max);

            PT_UNIT_ASSERT( circularBuffer.isEmpty() );
        }
};

Pt::Unit::RegisterTest<CircularBufferTest> register_CircularBufferTest;
