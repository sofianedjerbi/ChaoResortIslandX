var __b__, door, eat_snd, _alrm, _notrip, vertl, get_rideable, place_solid, no_ride, no_placeOn, boj, img, page, adj_d_mode;
with (objChaoHUD) {
    if (force_prompt == 0)
        d_mode_show = 0
}
if (flying == 1) {
    flying = 0
    if (scr_cw_collision(x, y) == 1)
        flying = 1
}
__b__ = action_if(action != "fly")
if __b__ {
    __b__ = action_if((global.exit_garden == 0 && global.enter_garden == 0 && global.chao_debug == 0))
    if __b__ {
        __b__ = action_if(in_hub == 1)
        if __b__ {
            if (no_input == 0) {
                if (place_meeting(x, (y - 25), objCWOrganizerB) && facing != "down" && carry == 0) {
                    action = "stand"
                    no_input = 1
                    if (global._event_scene >= global._evt_open) {
                        if (x > (room_width / 2))
                            instance_create(0, 0, objChaoOrganizer)
                        else
                            instance_create(0, 0, objBulletinBoard)
                    }
                    else
                        scr_init_self_talk(0, "")
                    with (objChaoHUD)
                        welcome_die = 1
                }
                else if (place_meeting((x + (16 * h_dir)), (y + (16 * v_dir)), objChaoMaster) && h_dir != -1) {
                    if (facing == "up" || facing == "down") {
                        x_shift = objChaoMaster.x
                        y_shift = (objChaoMaster.y - (18 * v_dir))
                    }
                    else {
                        y_shift = objChaoMaster.y
                        if (carry_id == self)
                            x_shift = (objChaoMaster.x - (26 * h_dir))
                        else
                            x_shift = (objChaoMaster.x - (35 * h_dir))
                    }
                    if (carry_id == self)
                        action = "listen"
                    else
                        action = "stand"
                    no_input = 1
                    scr_init_convo()
                    with (objChaoHUD)
                        welcome_die = 1
                }
            }
        }
        else {
            __b__ = action_if(global.d_mode == 0)
            if __b__ {
                fdir = (dir + face_dir)
                if (action != "jump" && action != "pickup" && action != "pet" && no_input == 0) {
                    if (carry == 0) {
                        pick = objGameBoy
                        cust = noone
                        door = instance_place(x, (y + (16 * v_dir)), objCWGateDoorMsk)
                        cust = instance_place((x + (16 * h_dir)), (y + (16 * v_dir)), objCust_Solid)
                        cust2 = instance_place((x + (16 * h_dir)), (y + (16 * v_dir)), objCust_Par)
                        pick = scr_cw_nearest2(objCWPlayerM.x, objCWPlayerM.y, objDistanceControl)
                        if (door != noone) {
                            with (door.gate) {
                                if (closed == 1) {
                                    closed = 0
                                    sprite_index = sprCWGateDO
                                    mask_index = mskCWGateDO
                                    audio_play_sound(sndCWGateOpen, 5, false)
                                }
                                else {
                                    mask_index = mskCWGateDC
                                    if ((!place_meeting(x, y, objCWSolid)) && (!place_meeting(x, y, objCWPlayer))) {
                                        closed = 1
                                        sprite_index = sprCWGateDC
                                        mask_index = sprCWGateDC
                                        audio_play_sound(sndCWGateClose, 5, false)
                                    }
                                    else
                                        mask_index = mskCWGateDO
                                }
                            }
                        }
                        else if (pick != objGameBoy) {
                            if (pick.owned == 0) {
                                alarm[0] = 13
                                action = "pickup"
                                pickup_anim = 19
                                with (pick) {
                                    if (is_chao == 1) {
                                        if (animation == "walk")
                                            animation = "stand"
                                        chao_speed = 0
                                    }
                                }
                            }
                        }
                        else if (cust != noone) {
                            if (cust.object_index == objCust_Chest) {
                                if (action == "move")
                                    action = "stand"
                                no_input = 1
                                with (cust) {
                                    open = 1
                                    alarm[1] = 20
                                }
                                audio_play_sound(sndCWChestOpen, 5, false)
                            }
                            else if (cust.object_index == objCust_Closet) {
                                if (action == "move")
                                    action = "stand"
                                no_input = 1
                                with (cust) {
                                    open = 1
                                    alarm[1] = 20
                                }
                                audio_play_sound(sndCWClosetOpen, 5, false)
                            }
                            else if (cust.object_index == objCust_Trash) {
                                if (action == "move")
                                    action = "stand"
                                no_input = 1
                                with (cust)
                                    alarm[0] = 20
                                audio_play_sound(sndCWTrashOpen, 5, false)
                            }
                            else if ((cust.object_index == objCust_Arcade1 || cust.object_index == objCust_Arcade2) && facing == "up") {
                                if (action == "move")
                                    action = "stand"
                                no_input = 1
                                with (cust)
                                    scr_cw_prompt((("Would you like to play " + gameName) + "?"), 2, "Yes", "No", "", "Y", 1)
                            }
                            else if (cust.object_index == objCust_PartyBooth && facing == "up") {
                                if (action == "move")
                                    action = "stand"
                                no_input = 1
                                with (cust)
                                    scr_cw_prompt("Would you like to play Fruit Maze?", 2, "Yes", "No", "", "Y", 1)
                                scr_cw_prompt_sub("Collect the most fruit for the hungry#chao before the time runs out!", 0, "", "", "", "Y", 0)
                            }
                            else if (cust.object_index == objCust_Snack && facing == "up") {
                                if (action == "move")
                                    action = "stand"
                                no_input = 1
                                with (cust)
                                    alarm[0] = 60
                                audio_play_sound(sndCWSnackUse, 5, false)
                            }
                        }
                        else if (cust2 != noone) {
                            if (cust2.object_index == objCWBerryBush) {
                                with (cust2) {
                                    if ((!place_meeting(x, (y + 1), objCWItemPar)) && myBerry.finish == 1 && global._item_amount[global.resort] < 40) {
                                        alarm[0] = 1
                                        audio_play_sound(sndBerryPick, 10, false)
                                    }
                                }
                            }
                            if (object_get_parent(cust2.object_index) == 298) {
                                with (cust2) {
                                    if (feature == 0) {
                                        feature = 1
                                        audio_play_sound(sndTurnOff, 10, false)
                                    }
                                    else {
                                        feature = 0
                                        audio_play_sound(sndTurnOn, 10, false)
                                    }
                                    global._custom_slot[custom_id, 4] = feature
                                }
                            }
                        }
                    }
                    else {
                        chao = scr_cw_nearest2(objCWPlayerM.x, objCWPlayerM.y, objChaoPar)
                        door = instance_place(x, (y + (16 * v_dir)), objCWGateDoorMsk)
                        if (door != noone) {
                            with (door.gate) {
                                if (closed == 1) {
                                    closed = 0
                                    sprite_index = sprCWGateDO
                                    mask_index = mskCWGateDO
                                    audio_play_sound(sndCWGateOpen, 5, false)
                                }
                                else {
                                    mask_index = sprCWGateDC
                                    if ((!place_meeting(x, y, objCWSolid)) && (!place_meeting(x, y, objCWPlayer))) {
                                        closed = 1
                                        sprite_index = sprCWGateDC
                                        mask_index = sprCWGateDC
                                        audio_play_sound(sndCWGateClose, 5, false)
                                    }
                                    else
                                        mask_index = mskCWGateDO
                                }
                            }
                        }
                        else if (carry_id.object_index == objCWFruit && chao != objGameBoy) {
                            if (chao.hatch_time == -1 && (chao.animation == "walk" || chao.animation == "stand" || chao.animation == "sit") && chao.stoolMode == 0) {
                                if (global._chao[chao.chao_id, 9] < 10) {
                                    if (global.player == "Tails")
                                        audio_play_sound(vcTGive, 5, false)
                                    with (chao) {
                                        scr_chao_emotion_reset(1)
                                        sd = sign((other.x - x))
                                        if (sd == 0)
                                            sd = choose(-1, 1)
                                        chao_dirx = sd
                                        chao_diry = 0
                                        animation = "eat"
                                        sound_instance_stop(chao_voice)
                                        eat_snd = choose(76, 82, 23, 23)
                                        chao_voice = sound_instance_play(chao_em, eat_snd)
                                        audio_play_sound(sndC_Take, 5, false)
                                        sound_delay = 40
                                        if (global._chao[chao_id, 9] < 2 || (prsn_hungry == 1 && global._chao[chao_id, 9] < 8)) {
                                            dd_sound = 97
                                            starving = 1
                                        }
                                        else if (global._chao[chao_id, 9] < 8)
                                            dd_sound = 77
                                        else {
                                            dd_sound = 131
                                            full = 1
                                        }
                                        food = other.carry_id
                                        emote = 6
                                        global._chao_id = chao_id
                                        scr_chao_happiness(2)
                                        scr_chao_color_scan_adv(global.player, 0.02)
                                        if (global._chao[chao_id, 3] < 2 && global._chao[chao_id, 2] < 500)
                                            global._chao[chao_id, 55] = min((global._chao[chao_id, 55] + 5), 9999)
                                        with (food) {
                                            if (other.prsn_hungry == 1)
                                                _mood = max(_mood, 0)
                                            else if (other.prsn_picky == 1 && _mood != objGameBoy)
                                                _mood -= 1
                                            else if (other.prsn_angry == 1 && _power != objGameBoy)
                                                _power += 1
                                            else if (other.prsn_misch == 1 && _run != objGameBoy)
                                                _run += 1
                                            else if (other.prsn_energetic == 1) {
                                                if (_power != objGameBoy)
                                                    _power += 1
                                                if (_run != objGameBoy)
                                                    _run += 1
                                            }
                                            scr_stat_influence_fruit()
                                            other.bar_amount = max(abs(_mood), abs(_belly), abs(_swim), abs(_fly), abs(_run), abs(_power), abs(_stamina))
                                            if (global._chao[other.chao_id, 3] == 1) {
                                                if (image_index == 9) {
                                                    with (other.id) {
                                                        scr_chao_color_scan("angel")
                                                        scr_chao_color_scan("angel")
                                                    }
                                                }
                                                else if (image_index == 10) {
                                                    with (other.id) {
                                                        scr_chao_color_scan("dark")
                                                        scr_chao_color_scan("dark")
                                                    }
                                                }
                                            }
                                            else if (global._chao[other.chao_id, 3] >= 2) {
                                                if (image_index == 14 && global._chao[other.chao_id, 2] < 500)
                                                    other.mate_force = other.mate_force_s
                                            }
                                            if (image_index == 11) {
                                                global._chao[other.chao_id, 6] += 5
                                                global._chao[other.chao_id, 6] = clamp(global._chao[other.chao_id, 6], -100, 100)
                                            }
                                        }
                                        alarm[3] = 60
                                        pick_up = 0
                                        mood_t = mood_s
                                        hunger_t = hunger_s
                                        temp_mood = (global._chao[chao_id, 8] + food._mood)
                                        temp_belly = (global._chao[chao_id, 9] + food._belly)
                                        temp_swim = (global._chao[chao_id, 10] + food._swim)
                                        temp_fly = (global._chao[chao_id, 11] + food._fly)
                                        temp_run = (global._chao[chao_id, 12] + food._run)
                                        temp_power = (global._chao[chao_id, 13] + food._power)
                                        temp_stamina = (global._chao[chao_id, 14] + food._stamina)
                                    }
                                    with (carry_id) {
                                        follow = 0
                                        z = z_h
                                        mask_index = temp_mask
                                        pick_up = 0
                                        x = (other.chao.x + (10 * other.chao.chao_dirx))
                                        y = other.chao.y
                                        ignore_depth = 1
                                        depth = (other.chao.depth + 0.5)
                                        destroy = 1
                                    }
                                    with (objCWStats)
                                        instance_destroy()
                                    with (instance_create(0, 0, objCWStats)) {
                                        timer = time_max
                                        food[0] = other.carry_id._mood
                                        food[1] = other.carry_id._belly
                                        food[2] = other.carry_id._swim
                                        food[3] = other.carry_id._fly
                                        food[4] = other.carry_id._run
                                        food[5] = other.carry_id._power
                                        food[6] = other.carry_id._stamina
                                    }
                                    carry_id = self
                                    carry = 0
                                    global._item_id = -1
                                    global._chao_id = -1
                                }
                                else {
                                    with (chao) {
                                        animation = "no"
                                        sound_instance_stop(chao_voice)
                                        eat_snd = choose(87, 88, 89, 23)
                                        chao_voice = sound_instance_play(chao_em, eat_snd)
                                        audio_play_sound(sndC_Take, 5, false)
                                        if (chao_dirx == 0)
                                            chao_dirx = choose(1, -1)
                                        chao_diry = 0
                                        emote = 5
                                        alarm[3] = 120
                                        pick_up = 0
                                    }
                                }
                                action = "pickup"
                                pickup_anim = 19
                            }
                        }
                        else if (carry_id.object_index == objCWDrives && chao != objGameBoy) {
                            if (chao.hatch_time == -1 && (chao.animation == "walk" || chao.animation == "stand" || chao.animation == "sit") && chao.stoolMode == 0) {
                                if (global.player == "Tails")
                                    audio_play_sound(vcTGive, 5, false)
                                with (chao) {
                                    scr_chao_emotion_reset(1)
                                    sd = sign((other.x - x))
                                    if (sd == 0)
                                        sd = choose(-1, 1)
                                    chao_dirx = sd
                                    chao_diry = 0
                                    animation = "reach"
                                    audio_play_sound(sndC_Take, 5, false)
                                    audio_play_sound(sndC_Absorb, 5, false)
                                    food = other.carry_id
                                    with (food) {
                                        if (other.prsn_angry == 1 && _power > objGameBoy)
                                            _power += 1
                                        else if (other.prsn_misch == 1 && _run > objGameBoy)
                                            _run += 1
                                        else if (other.prsn_energetic == 1) {
                                            if (_power > objGameBoy)
                                                _power += 1
                                            if (_run > objGameBoy)
                                                _run += 1
                                        }
                                        scr_stat_influence()
                                        other.bar_amount = max(abs(_swim), abs(_fly), abs(_run), abs(_power))
                                    }
                                    if (happiness_ts == 0) {
                                        scr_chao_happiness(2)
                                        happiness_ts = happiness_ss
                                    }
                                    scr_chao_color_scan_adv(global.player, 0.02)
                                    global._chao_id = chao_id
                                    alarm[1] = 140
                                    alarm[3] = 60
                                    alarm[2] = 20
                                    pick_up = 0
                                    temp_swim = (global._chao[chao_id, 10] + food._swim)
                                    temp_fly = (global._chao[chao_id, 11] + food._fly)
                                    temp_run = (global._chao[chao_id, 12] + food._run)
                                    temp_power = (global._chao[chao_id, 13] + food._power)
                                }
                                with (carry_id) {
                                    follow = 0
                                    z_h += 10
                                    z = z_h
                                    mask_index = temp_mask
                                    pick_up = 0
                                    x = (other.chao.x + (18 * other.chao.chao_dirx))
                                    y = other.chao.y
                                    ignore_depth = 1
                                    depth = (other.chao.depth + 0.5)
                                    destroy = 1
                                }
                                with (objCWStats)
                                    instance_destroy()
                                with (instance_create(0, 0, objCWStats)) {
                                    timer = time_max
                                    food[2] = other.carry_id._swim
                                    food[3] = other.carry_id._fly
                                    food[4] = other.carry_id._run
                                    food[5] = other.carry_id._power
                                }
                                carry_id = self
                                carry = 0
                                action = "pickup"
                                pickup_anim = 19
                                global._item_id = -1
                                global._chao_id = -1
                            }
                        }
                        else if ((carry_id.object_index == objToyTrumpet || carry_id.object_index == objCWDoll || carry_id.object_index == objCWDoll2) && chao != objGameBoy) {
                            if (chao.hatch_time == -1 && (chao.animation == "walk" || chao.animation == "stand" || chao.animation == "sit") && chao.stoolMode == 0) {
                                with (chao) {
                                    food = other.carry_id
                                    if (food.object_index == objToyTrumpet) {
                                        scr_chao_emotion_reset(1)
                                        audio_play_sound(sndC_Take, 5, false)
                                        sd = sign((other.x - x))
                                        if (sd == 0)
                                            sd = choose(-1, 1)
                                        chao_dirx = sd
                                        chao_diry = 0
                                        animation = "horn"
                                        emote = 6
                                        pick_up = 0
                                        sound_delay = 20
                                        if (global._chao[chao_id, 35] < 5) {
                                            dd_sound = 161
                                            alarm[3] = 160
                                          }
                                        else if (global._chao[chao_id, 35] < 10) {
                                            dd_sound = 162
                                            alarm[3] = 160
                                          }
                                        else if (global._chao[chao_id, 35] < 20) {
                                            dd_sound = 163
                                            alarm[3] = 160
                                          }
                                        else if (global._chao[chao_id, 35] < 35) {
                                            dd_sound = asset_get_index("sndHorn4")
                                            alarm[3] = 320
                                          }
                                        else if (global._chao[chao_id, 35] < 50) {
                                            dd_sound = asset_get_index("sndHorn5")
                                            alarm[3] = 275
                                          }
                                        else {
                                            dd_sound = asset_get_index("sndHorn6")
                                            alarm[3] = 715
                                          }
                                        if (happiness_tl == 0) {
                                            scr_chao_mood((2 + prsn_music))
                                            scr_chao_happiness((2 + prsn_music))
                                            happiness_tl = happiness_sl
                                            scr_chao_color_scan_adv(global.player, 0.02)
                                        }
                                        if (global._chao[chao_id, 35] < 60) {
                                            if (prsn_misch == 1 || prsn_music == 1)
                                                global._chao[chao_id, 35] += 1
                                            else if (prsn_angry == 1)
                                                global._chao[chao_id, 35] += 0.3
                                            else
                                                global._chao[chao_id, 35] += 0.5
                                        }
                                    }
                                    else {
                                        scr_chao_emotion_reset(1)
                                        audio_play_sound(sndC_Take, 5, false)
                                        chao_dirx = 0
                                        chao_diry = 1
                                        animation = choose("dance2", "happy-sit")
                                        scr_chao_eyes(5)
                                        emote = 6
                                        _alrm = (irandom(120) + 260)
                                        alarm[3] = _alrm
                                        alarm[1] = (_alrm + 60)
                                        pick_up = 0
                                        if (happiness_tl == 0) {
                                            scr_chao_mood((2 + prsn_crybaby))
                                            scr_chao_happiness((2 + prsn_crybaby))
                                            happiness_tl = happiness_sl
                                            if (food.object_index == objCWDoll)
                                                scr_chao_color_scan_adv("Sonic", 0.05)
                                            else
                                                scr_chao_color_scan_adv("Shadow", 0.05)
                                        }
                                    }
                                }
                                with (carry_id) {
                                    if (object_index == objToyTrumpet) {
                                        follow = 0
                                        z = (z_h + 4)
                                        mask_index = temp_mask
                                        pick_up = 0
                                        x = (other.chao.x + (15 * other.chao.chao_dirx))
                                        y = other.chao.y
                                        xscale = other.chao.chao_dirx
                                        ignore_depth = 1
                                        depth = (other.chao.depth + 0.5)
                                        image_index = 0.15
                                        image_speed = 0.05
                                    }
                                    else {
                                        follow = 0
                                        z = (z_h + 4)
                                        mask_index = temp_mask
                                        pick_up = 0
                                        x = (other.chao.x - 11)
                                        y = other.chao.y
                                        ignore_depth = 1
                                        depth = (other.chao.depth + 0.5)
                                    }
                                    global._item_slot[item_id, 1] = x
                                    global._item_slot[item_id, 2] = y
                                }
                                carry_id = self
                                carry = 0
                                action = "pickup"
                                pickup_anim = 19
                                global._item_id = -1
                                global._chao_id = -1
                            }
                        }
                        else if ((carry_id.object_index == objCWH_Model || carry_id.object_index == objCWA_Model) && chao != objGameBoy) {
                            if (chao.hatch_time == -1 && (chao.animation == "walk" || chao.animation == "stand" || chao.animation == "sit") && chao.stoolMode == 0) {
                                scr_chao_emotion_reset(1)
                                was_wear = -1
                                with (chao) {
                                    chao_dirx = 0
                                    chao_diry = 1
                                    sound_delay = 10
                                    if (other.carry_id.object_index == objCWH_Model) {
                                        animation = "think-2"
                                        if (hat_wear > -1)
                                            other.was_wear = global._chao[chao_id, 42]
                                        hat_wear = other.carry_id.spr_name
                                        global._chao[chao_id, 42] = other.carry_id.feature
                                        hat_y = 3
                                        dd_sound = choose(84, 85, 86, 23)
                                    }
                                    else {
                                        animation = "think-1"
                                        if (acc_wear > -1)
                                            other.was_wear = global._chao[chao_id, 43]
                                        acc_wear = other.carry_id.spr_name
                                        global._chao[chao_id, 43] = other.carry_id.feature
                                        dd_sound = choose(84, 85, 85, 23, 23)
                                    }
                                    emote = 3
                                    alarm[3] = 160
                                    pick_up = 0
                                }
                                with (carry_id) {
                                    if (other.was_wear > -1) {
                                        feature = other.was_wear
                                        alarm[0] = 1
                                        alarm[11] = 2
                                    }
                                    else
                                        instance_destroy()
                                }
                                if (was_wear == -1) {
                                    carry_id = self
                                    carry = 0
                                    global._item_id = -1
                                }
                                action = "pickup"
                                pickup_anim = 19
                            }
                        }
                        else {
                            with (carry_id) {
                                _notrip = 1
                                vertl = (16 * other.v_dir)
                                vertl = max(vertl, -12)
                                mask_index = temp_mask
                                get_rideable = instance_place((other.x + (24 * other.h_dir)), (other.y + vertl), objCWItemPar)
                                place_solid = instance_place((other.x + (24 * other.h_dir)), (other.y + vertl), objCWSolid)
                                no_ride = 0
                                no_placeOn = 0
                                if get_rideable {
                                    no_ride = 1
                                    if (get_rideable.is_rideable == 1 && get_rideable.drive == 0 && is_chao == 1)
                                        no_ride = 2
                                }
                                if place_solid {
                                    if (place_solid.put_solid == 1)
                                        no_placeOn = 1
                                }
                                if (((!place_meeting((other.x + (24 * other.h_dir)), (other.y + vertl), objCWSolid)) || no_placeOn == 1) && (!place_meeting(other.x, (other.y + vertl), objCWOceanBlock))) {
                                    if (no_ride == 0 || no_ride == 2) {
                                        if (other.spdup > 1 && other.moving == 1 && global._event_scene >= global._evt_open) {
                                            if (is_chao == 1) {
                                                if (hatch_time == -1) {
                                                    sound_instance_stop(chao_voice)
                                                    scr_chao_throw()
                                                }
                                                else
                                                    scr_egg_throw()
                                                global._chao_id = -1
                                                with (other.id) {
                                                    carry = 0
                                                    action = "stand"
                                                    stop_time = 10
                                                    carry_id = self
                                                }
                                                _notrip = 0
                                                follow = 0
                                                if (other.h_dir == 0) {
                                                    if (other.v_dir == -1)
                                                        y = (other.y - 12)
                                                    else
                                                        y = (other.y + 16)
                                                }
                                                else
                                                    y = other.y
                                                x = (other.x + (24 * other.h_dir))
                                                ystart = y
                                                with (objCWStats)
                                                    destroy = 1
                                            }
                                            else {
                                                scr_item_throw()
                                                thrown = 1
                                                follow = 0
                                                global._item_id = -1
                                                with (other.id) {
                                                    carry = 0
                                                    action = "stand"
                                                    carry_id = self
                                                }
                                                if (other.h_dir == 0) {
                                                    if (other.v_dir == -1)
                                                        y = (other.y - 12)
                                                    else
                                                        y = (other.y + 16)
                                                }
                                                else
                                                    y = other.y
                                                x = (other.x + (24 * other.h_dir))
                                                _notrip = 0
                                            }
                                        }
                                        else {
                                            other.alarm[0] = 13
                                            other.action = "pickup"
                                            other.pickup_anim = 19
                                        }
                                    }
                                }
                                if (_notrip == 1)
                                    mask_index = nomask
                            }
                        }
                    }
                }
            }
            __b__ = action_if(global.d_mode == 1)
            if __b__ {
                pick = objGameBoy
                pick = scr_cw_nearest2(objCWPlayerM.x, objCWPlayerM.y, objCWItemPar)
                if (pick != objGameBoy && carry == 0 && no_input == 0) {
                    if (pick.switchable == 1) {
                        with (pick) {
                            if (feature == 1) {
                                audio_play_sound(sndTurnOff, 10, false)
                                feature = 0
                                global._item_slot[item_id, 4] = feature
                                alarm[0] = 1
                                if (object_index == objToyTV || object_index == objToyRadio) {
                                    if (channel > -1)
                                        audio_sound_gain(global.BGM, 1, 600)
                                    audio_stop_sound(static)
                                    audio_stop_sound(tv_music)
                                    audio_stop_sound(tv_sound)
                                    static = -1
                                }
                            }
                            else {
                                audio_play_sound(sndTurnOn, 10, false)
                                feature = 1
                                global._item_slot[item_id, 4] = feature
                                alarm[0] = 1
                            }
                        }
                    }
                }
                if (no_input == 0 && carry == 1) {
                    if (global._chao_id > -1 && global._chao[global._chao_id, 3] > 0) {
                        scr_cw_prompt("What would you like to name this chao?", 2, "Enter", "Cancel", "", "T", 0)
                        if (global._chao[global._chao_id, 1] != "No name")
                            keyboard_string = global._chao[global._chao_id, 1]
                        no_input = 1
                        action = "stand"
                    }
                    else if (global._item_id > -1) {
                        if (carry_id.backp_id > -1) {
                            boj = carry_id.object_index
                            if (carry_id.backp_id >= 3)
                                img = carry_id.feature
                            else
                                img = carry_id.image_index
                            page = carry_id.backp_id
                            global._backpack[global._bp_amount, 0] = boj
                            global._backpack[global._bp_amount, 1] = img
                            global._backpack[global._bp_amount, 2] = page
                            with (objChaoControl) {
                                bp_img[global._bp_amount] = global._backpack[global._bp_amount, 2]
                                bp_ext[global._bp_amount] = 0
                            }
                            global._bp_amount += 1
                            with (carry_id)
                                instance_destroy()
                            global._item_id = -1
                            carry_id = -1
                            carry = 0
                            action = "stand"
                            audio_play_sound(sndC_PocketIn, 10, false)
                        }
                    }
                }
            }
            __b__ = action_if(global.d_mode > 1)
            if __b__ {
                adj_d_mode = global.d_mode
                if (global.d_mode == 2)
                    target_cloth = objCWH_Model
                else
                    target_cloth = objCWA_Model
                if (global._item_amount[global.resort] < 40) {
                    pick = objGameBoy
                    pick = scr_cw_nearest2(objCWPlayerM.x, objCWPlayerM.y, objChaoPar)
                    if (pick != objGameBoy && carry == 0 && no_input == 0) {
                        if ((pick.hat_wear > -1 && adj_d_mode == 2) || (pick.acc_wear > -1 && adj_d_mode == 3)) {
                            c_obj = instance_create(x, y, target_cloth)
                            c_obj.item_id = (global._item_amount[global.resort] + global.resort_i)
                            global._item_id = c_obj.item_id
                            global._item_amount[global.resort] += 1
                            with (pick) {
                                if (other.target_cloth == 2) {
                                    other.c_obj.feature = global._chao[chao_id, 42]
                                    hat_wear = -1
                                    global._chao[chao_id, 42] = -1
                                    hat_y = 0
                                }
                                else {
                                    other.c_obj.feature = global._chao[chao_id, 43]
                                    acc_wear = -1
                                    global._chao[chao_id, 43] = -1
                                }
                            }
                            with (c_obj) {
                                follow = 1
                                mask_index = nomask
                            }
                            carry = 1
                            carry_id = c_obj
                            c_obj = -1
                        }
                    }
                }
            }
        }
        if (global.player == "Tails" && no_input == 0) {
            if (objChaoHUD.local_d_mode[global.d_mode] == "Fly") {
                if (flying == 0 && (action == "stand" || action == "move")) {
                    action = "fly"
                    flying = 1
                    if (!audio_is_playing(sndTailsFly))
                        audio_play_sound(sndTailsFly, 5, true)
                    cut_off = 0
                    voluntOption = 0
                }
            }
        }
    }
}
