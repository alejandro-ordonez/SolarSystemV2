react = reactor;
disp('Meta esa madre');
react.T= input('Temperatura en K');
react.C = input('Meta la C');

%%Ciclo de repetitivo de opciones
while true
    disp('1. Presion Maxima');
    disp('2. Presion Minima');
    disp('3. Salir');
    op = input('Ingrese la opcion');
    switch op
        case 1
            %El valor de react se actualiza
            react = calculateTSonopolus(react);
            disp(react.B(1));
            break;
        case 2
            break;
        case 3
            break;
    end
end


%% Functions


function y = calculateTSonopolus(R)
    R.B(0) = R.T*R.Tr;
    R.B(1) = R.C*R.A;
    y = R;
end

%%