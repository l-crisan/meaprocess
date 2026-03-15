/***************************************************************************
*   Copyright (C) 2006-2007 by Laurentiu-Gheorghe Crisan                  *
*                                                                         *
*   All rights reserved.                                                  *
***************************************************************************/

#undef PT_API_EXPORT
#include <Pt/Unit/TestMain.h>
#include <Pt/Unit/Assertion.h>
#include <Pt/Unit/TestSuite.h>
#include <Pt/Unit/RegisterTest.h>
#include <vector>
#include <mps/candrv/CANDriver.h>



class CANDrvTest : public Pt::Unit::TestSuite
{
    public:
        CANDrvTest()
        : Pt::Unit::TestSuite("CANDrvTest")
        {
            Pt::Unit::TestSuite::registerMethod( "testCANDriver", *this, &CANDrvTest::testCANDriver );
			//Pt::Unit::TestSuite::registerMethod( "testCANTP", *this, &CANDrvTest::testCANTP );
			//Pt::Unit::TestSuite::registerMethod( "testOBD2", *this, &CANDrvTest::testOBD2 );
        }

    protected:

		void printMessage(const mps::candrv::Message& msg)
		{
			std::cout<<std::hex<<std::uppercase<<msg.identifier() <<" ";
			std::cout<<(int)msg.dlc()<<" ";
				
			for( size_t i = 0; i < 8; ++i)
				std::cout<<(int)msg.data()[i]<<" ";

			std::cout<<std::endl;
		}

		void printData(const std::vector<Pt::uint8_t>& data, bool hex = false)
		{
			if(hex)
				std::cout<<std::hex<<std::uppercase;
			else
				std::cout<<std::dec;

			for( size_t i = 0; i < data.size(); ++i)
			{
				if( hex)
					std::cout<<(int)data[i]<<" ";
				else
					std::cout<<data[i]<<" ";
			}

			std::cout<<std::endl;
		}

		void testOBD2()
		{/*
			std::cout<<std::endl;
			CANDriver driver;
			
			if(!driver.open(4, 500000,CANDriver::ELM327))//Port 4, 500 kB baudrate
			{
				std::cerr << "Open can driver faild"<<std::endl;				
				return;
			}

			driver.setAcceptanceMask(0xFF0,0x7E0); //mask, code
			TPCanHandler canHandler(driver);			
			TPDUAddress  address(0x7E0, driver.extendedID()); //ECU1=0x7E0, Functional=0x7DF
			
			EngineRPM obd2request(canHandler,address);
			printData(obd2request.getResponse(),true);
			printData(obd2request.getResponse(),true);

			driver.close();
		*/
		}

        void testCANTP()
        {/*
			std::cout<<std::endl;
			
			CANDriver driver;
			
			if(!driver.open(4, 500000, CANDriver::ELM327))//Port 4, 500 kB baudrate
			{
				std::cerr << "Open can driver faild"<<std::endl;
				return;
			}
		
			TPCanHandler canHandler(driver);			
			TPDUAddress  address(0x7E0, driver.extendedID()); //ECU1=0x7E0, Functional=0x7DF
			
			Pt::uint8_t data[2] = {0x09, 0x02}; //SID=0x09(Car information) PID=0x02(Car serial number)
		
			TPRequest request(canHandler, address, data, 2);
			request.request();
			if(!request.waitResponse(1000))
			{
				driver.close();
				std::cerr << "receive timeout"<<std::endl;
				return;
			}

			const std::vector<Pt::uint8_t>& rspData = request.response();						
			printData(rspData);

			driver.close();*/
        }

		void testCANDriver()
        {
			/*
			std::cout<<std::endl;
			
			CANDriver driver;
			
			if(!driver.open(4, 500000, CANDriver::ELM327, false))
			{
				std::cout << "Open can driver faild"<<std::endl;
				return;
			}

			CANDriver driver1;
			
			if(!driver1.open(4, 500000, CANDriver::ELM327, false))
			{
				std::cout << "Open can driver faild"<<std::endl;
				return;
			}

			CANDriver driver2;
			
			if(!driver2.open(2, 500000, CANDriver::ELM327, false))
			{
				std::cout << "Open can driver faild"<<std::endl;
				return;
			}

/*
			//Send first request
			Pt::uint8_t data[8] = { 0x02, 0x09, 0x02 }; //SF=0x02 SID=0x09 PID=0x02
			
			Message msg(0x7E0, data, 8);
			driver.send(msg);

			if(!driver.wait(10000))
			{
				std::cout << "receive first frame timeout 1"<<std::endl;
				driver.close();
				return;
			}
			
			Message rcvMsg;
			if(!driver.receive(rcvMsg))
			{
				std::cout << "receive first frame error"<<std::endl;
				driver.close();
				return;
			}

			printMessage(rcvMsg);
		
			//Send flow control
			memset(msg.data(),0,8);
			msg.setIdentifier(0x7E0);

			data[0] = 0x30;
			data[1] = 0;
			data[2] = 100;

			memcpy(msg.data(), data,3);
			driver.send(msg);

			while(driver.wait(5000))
			{
				driver.receive(rcvMsg);
				printMessage(rcvMsg);
			}
*/
//			driver.close();
        }
};

Pt::Unit::RegisterTest<CANDrvTest> register_CANDrvTest;
