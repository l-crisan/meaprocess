// Copyright (C) 2010-2016 by ATESiON GmbH. All rights reserved.
#include <mps/drv/opencv/Driver.h>
#include <opencv2/opencv.hpp>
#include <Pt/System/Clock.h>

extern "C"
{

static int detectFrameRate(cv::VideoCapture* capture)
{
    const int width = (int) capture->get(CV_CAP_PROP_FRAME_WIDTH);
    const int height = (int) capture->get(CV_CAP_PROP_FRAME_HEIGHT);	
    const Pt::uint32_t size = width*height*3;
    const Pt::uint32_t frames = 100;
    std::vector<char> buffer(size);
    
    double sum  =  0;

    //Grab 20 frames and measure the time
    Pt::System::Clock clock;

    for( Pt::uint32_t i = 0; i < frames; ++i)
    {
        clock.start();
        mpsopencv_readFrame((Pt::uint64_t) capture, &buffer[0]);
        Pt::Timespan spam = clock.stop();
        sum += (1000.0/spam.toMSecs());
    }
 
    return (int) (sum/frames);
}

MPS_OPEN_API Pt::uint64_t mpsopencv_open(int card)
{
    cv::VideoCapture* capture = 0;
    try
    {
        capture = new cv::VideoCapture(card);
        capture->set(CV_CAP_PROP_CONVERT_RGB ,1);
    }
    catch(...)
    {
        return 0;
    }
    
    return (Pt::uint64_t) capture;
}

MPS_OPEN_API int mpsopencv_readFrame(Pt::uint64_t handle, char* buffer)
{
    cv::VideoCapture* capture = (cv::VideoCapture*) handle;	
    cv::Mat frame;

    const Pt::uint32_t width = (Pt::uint32_t) capture->get(CV_CAP_PROP_FRAME_WIDTH);
    const Pt::uint32_t height = (Pt::uint32_t) capture->get(CV_CAP_PROP_FRAME_HEIGHT);	
    const Pt::uint32_t size = width*height*3;

    if(!capture->read(frame))
        return 0;
    
    memcpy(buffer, frame.ptr<cv::Point3_<uchar> >(0,0), size);

    return (int) size;
}

MPS_OPEN_API void mpsopencv_close(Pt::uint64_t handle)
{
    cv::VideoCapture* capture = (cv::VideoCapture*) handle;	
    capture->release();
}

MPS_OPEN_API void mpsopencv_setFrameRate(Pt::uint64_t handle, int rate)
{
    cv::VideoCapture* capture = (cv::VideoCapture*) handle;	
    capture->set(CV_CAP_PROP_FPS, rate);
}

MPS_OPEN_API mpsopencv_DeviceInfo mpsopencv_detect(int card)
{	
    mpsopencv_DeviceInfo info;
    info.Error = 0;
    info.CardID = card;
    
    try
    {
        cv::VideoCapture capture(card);

        info.Rate = (int) capture.get(CV_CAP_PROP_FPS);

        if( info.Rate == 0)
            info.Rate = detectFrameRate(&capture);

        info.Width = (int) capture.get(CV_CAP_PROP_FRAME_WIDTH);
        info.Height = (int) capture.get(CV_CAP_PROP_FRAME_HEIGHT);	
        capture.release();	
    }
    catch(...)
    {
        info.Error = -1;
    }

    return info;

}

}
