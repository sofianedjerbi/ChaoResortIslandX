garden_model(room, "load")
global.exit_garden = 0
instance_create(0, 0, objCWTransition)
global.flat_mode = 1
arr_ = 0
alarm[1] = 30
scaleL = 1
scaleR = 1
scale_S = 1.8
alpha = 1
alpha2 = 1
die = 0
size = 1
size_p = 0
name_mov = 0
name_max = 500
name_s = 1
b_alpha = -4
alpha = -8
alpha2 = alpha
if (global.chao_time == 0)
    global.day_color = 16777215
else if (global.chao_time == 1)
    global.day_color = 4235519
else
    global.day_color = merge_color(c_navy, c_blue, 0.2)
if (global.weather_fx == 1) {
    if (global.chao_weather == 1)
        instance_create(0, 0, objCWLeaves)
    else if (global.chao_weather == 2)
        instance_create(0, 0, objCWSnow)
    else if (global.chao_weather == 3)
        instance_create(0, 0, objCWRain)
    else if (global.chao_weather == 4)
        instance_create(0, 0, objCWPetals)
}
no_input = 1
alarm[0] = 90
sel_char = 0
sel_char_o = 0
i = 0
char_set[i, 0] = "Sonic"
char_set[i, 1] = 0
i += 1
char_set[i, 0] = "Shadow"
char_set[i, 1] = 1
i += 1
char_set[i, 0] = "Knuckles"
char_set[i, 1] = 2
i += 1
char_set[i, 0] = "Rouge"
char_set[i, 1] = 1
i += 1
char_set[i, 0] = "Tails"
char_set[i, 1] = 0
i += 1
char_set[i, 0] = "Cream"
char_set[i, 1] = 0
i += 1
char_set[i, 0] = "Tikal"
char_set[i, 1] = 2
i += 1
char_set[i, 0] = "Chaos"
char_set[i, 1] = 2
i += 1
char_set[i, 0] = "Blaze"
char_set[i, 1] = 0
i += 1
char_set[i, 0] = "Metal"
char_set[i, 1] = 1

if (global._un_resort_c == 1) {
  max_char = 6
}
else {
  max_char = 5
}

switch global.player {
    case "Shadow":
        sel_char = 1
        break
    case "Knuckles":
        sel_char = 2
        break
    case "Rouge":
        sel_char = 3
        break
    case "Tails":
        sel_char = 4
        break
    case "Cream":
        sel_char = 5
        break
    case "Tikal":
        sel_char = 6
        break
    case "Chaos":
        sel_char = 7
        break
    case "Blaze":
        sel_char = 8
        break
    case "Metal":
        sel_char = 9
        break
    default:
        sel_char = 0
        break
}

char_x_pos = ((room_width / 2) - 3)
char_y_pos = 80
spr_title[0] = 215
spr_title[1] = 216
spr_title[2] = 216
xmov = -240
xmax = 0
t_alpha = -0.8
