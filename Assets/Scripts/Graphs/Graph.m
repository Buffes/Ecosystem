Array = csvread('GraphValues.csv');
col1 = Array(:,1);
col2 = Array(:,2);
col3 = Array(:,3);
p = plot(col1,col2, col1,col3);
p(1).LineWidth = 1.5;
p(2).LineWidth = 1.5;
names={'Chickens', 'Lions'};
legend(p, names);
set(gca,'FontSize',12)
xlabel('Time');
ylabel('#Animals');



