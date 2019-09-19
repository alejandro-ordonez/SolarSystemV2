# RC low pass filter
In order to improve the voltage signal of Pin 32 of ESP32 that measures the current sensor output of 
the ACS712 module. A first-order RC low pass filter was implemented.

#### The following equation models even first order low pass filter


 $\frac{x^2+1}{x-1}$.
