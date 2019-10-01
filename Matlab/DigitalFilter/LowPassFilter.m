format long
w_lp=5*2*pi;
s = tf(w_lp, [1, w_lp])
dt = 0.7;
%dt = 600*0.000001;
z = c2d(s,dt,'tustin')
b=squeeze(z.num)
a=squeeze(z.den)