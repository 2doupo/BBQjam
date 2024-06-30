extends Control

func set_value(val_act,val_tot): 
	$DecimalAct.int_to_frame(val_act%10)
	$DecimalMax.int_to_frame(val_tot%10)
	$DizaineAct.int_to_frame((val_act-(val_act%10))/10)
	$DizaineMax.int_to_frame((val_tot-(val_tot%10))/10)
