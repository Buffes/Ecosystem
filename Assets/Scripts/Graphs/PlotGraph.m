a = erase(pwd, "\Assets\Scripts\Graphs");
file =[a, "Stats\SpeedDoc.csv"];
file = join(file, "\");
SpeedArray = readtable(file);
figure(1)
PlotValues(GetValues(SpeedArray), SpeedArray,'Average Speed');

file = [a, "Stats\HearingDoc.csv"];
file = join(file, "\");
HearingArray = readtable(file);
figure(2)
PlotValues(GetValues(HearingArray), HearingArray, 'Average Hearing Range');

file = [a, "Stats\VisionDoc.csv"];
file = join(file, "\");
VisionArray = readtable(file);
figure(3)
PlotValues(GetValues(VisionArray), VisionArray, 'Average Vision Range');

file = [a, "Stats\AnimalCountDoc.csv"];
file = join(file, "\");
AnimalCountArray = readtable(file);
figure(4)
PlotValues(GetValues(AnimalCountArray), AnimalCountArray, 'Animal Count');

function [TempArray] = GetValues(Array)
   col1 = Array(:,1);
   col2 = Array(:,2);
   col3 = Array(:,3);
   l1 = length(table2array(col1));
   C = length(unique(table2array(col1)));
   B = length(unique(table2array(col2)));
   pos = 1;
   TempArray = zeros(C, B+1);
   animals = cell(1,B);
   column = 0;
   
   for i = 1:1:B
       animals(1,i) = table2array(col2(i,1));
   end

    for i = 1:1:l1    
        for j = 1:1:B
            if isequal(table2array(col2(i,1)),animals(1,j))
               column = j+1;
            end
        end
        TempArray(pos,column) = table2array(col3(i,1));
        if i == l1
            TempArray(pos,1) = table2array(col1(i,1));
            pos = pos + 1;
        elseif ~isequal(col1(i,1),col1(i+1,1))

            TempArray(pos,1) = table2array(col1(i,1));
            pos = pos + 1;
        end
    end
end

function [] = PlotValues(TempArray, Array, Ylabel)
    [~,columns]= size(TempArray);
    NameCol = Array(:,2);
    lx = find(TempArray(:,2)==0,1,'first');
    NoZeroArray = TempArray(1:lx-1,2);
    
    if lx ~= 0  
        plot(TempArray(1:lx-1,1),NoZeroArray, 'DisplayName', join(char(table2cell(NameCol(1,1)))));
        legend(join(char(table2cell(NameCol(1,1)))));
        hold on
    else       
        plot(TempArray(:,1),TempArray(:,2), 'DisplayName', join(char(table2cell(NameCol(1,1)))));
        legend(join(char(table2cell(NameCol(1,1)))));
        hold on
    end

    
    for i = 2:1:columns
       
        if columns > i
            
            lx = find(TempArray(:,i+1)==0,1,'first');
            NoZeroArray = TempArray(1:lx-1,i+1);
        
            if lx ~= 0  
                plot(TempArray(1:lx-1,1),NoZeroArray, 'DisplayName', join(char(table2cell(NameCol(i,1)))));
            else       
                plot(TempArray(:,1),TempArray(:,i+1), 'DisplayName', join(char(table2cell(NameCol(i,1)))));
            end
        end  
    end
    ylabel(Ylabel)
    xlabel('Time')      
end
    