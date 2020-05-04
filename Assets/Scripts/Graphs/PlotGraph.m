clc;
a = erase(pwd, "Ecosystem/Assets/Scripts/Graphs");
file =[a, "Ecosystem/Stats/SpeedDoc.csv"];
file = join(file, "/");
SpeedArray = readtable(file);
figure(1)
PlotValues(GetValues(SpeedArray), SpeedArray,'Average Speed');

file = [a, "Ecosystem/Stats/HearingDoc.csv"];
file = join(file, "/");
HearingArray = readtable(file);
figure(2)
PlotValues(GetValues(HearingArray), HearingArray, 'Average Hearing Range');

file = [a, "Ecosystem/Stats/VisionDoc.csv"];
file = join(file, "/");
VisionArray = readtable(file);
figure(3)
PlotValues(GetValues(VisionArray), VisionArray, 'Average Vision Range');

file = [a, "Ecosystem/Stats/AnimalCountDoc.csv"];
file = join(file, "/");
AnimalCountArray = readtable(file);
figure(4)
PlotValues(GetValues(AnimalCountArray), AnimalCountArray, 'Animal Count');

file = [a, "Ecosystem/Stats/AgeDoc.csv"];
file = join(file, "/");
AgeArray = readtable(file);
figure(5)
PlotValues(GetValues(AgeArray), AgeArray, 'Animal Ages');

file = [a, "Ecosystem/Stats/HungerLimitDoc.csv"];
file = join(file, "/");
AgeArray = readtable(file);
figure(6)
PlotValues(GetValues(AgeArray), AgeArray, 'Hunger Limit');

file = [a, "Ecosystem/Stats/ThirstLimitDoc.csv"];
file = join(file, "/");
AgeArray = readtable(file);
figure(7)
PlotValues(GetValues(AgeArray), AgeArray, 'Thirst Limit');

file = [a, "Ecosystem/Stats/MatingLimitDoc.csv"];
file = join(file, "/");
AgeArray = readtable(file);
figure(8)
PlotValues(GetValues(AgeArray), AgeArray, 'Mating Limit');

function [TempArray] = GetValues(Array)
   col1 = Array(:,1);
   col2 = Array(:,2);
   col3 = Array(:,3);
   C = length(unique(table2array(col1)));
   B = length(unique(table2array(col2)));
   pos = 1;
   TempArray = zeros(C, B+1);
   j_end = length(table2array(col1));
   
   disp(length(table2array(col1)))
   disp(C)
   
    for i = 1:length(table2array(col1))
        if i>B && table2array(col1(i,1))==0
            j_end = i-1;
            disp(TempArray)
            disp(j_end)
            TempArray = TempArray(1:j_end,:,:);
            break;
        end
        
        TempArray(pos,B - mod(i,B) +1) = table2array(col3(i,1));

        if mod(i,B) == 0
            TempArray(pos,1) = table2array(col1(i,1));
            pos = pos + 1;
        end     
    end
    %disp(TempArray)
end

function [] = PlotValues(TempArray, Array, Ylabel)
    [~,columns]= size(TempArray);
    NameCol = Array(:,2);
    plot(TempArray(:,1),TempArray(:,2), 'DisplayName', join(char(table2cell(NameCol(1,1)))),'LineWidth',2);
    legend(join(char(table2cell(NameCol(1,1)))),'FontSize',20);
    hold on
    
    for i = 2:1:columns
       
        if columns > i
            plot(TempArray(:,1),TempArray(:,i+1), 'DisplayName', join(char(table2cell(NameCol(i,1)))),'LineWidth',2);
        end
        
    end
    ylabel(Ylabel,'FontSize',20)
    xlabel('Time (in seconds)','FontSize',20)   
    hold off
end
    