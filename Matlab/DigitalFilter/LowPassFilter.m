format long
w_lp=15*2*pi;
s = tf(w_lp, [1, w_lp])
dt = 0.1;
z = c2d(s,dt,'tustin')
b=squeeze(z.num)
a=squeeze(z.den)