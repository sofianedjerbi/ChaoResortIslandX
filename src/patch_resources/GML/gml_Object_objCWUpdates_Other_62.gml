var __b__, chao_vt, get_u, status, path, files, mod_ver;
__b__ = action_if(cancel_con == 0)
if __b__ {
    if (ds_map_find_value(async_load, "id") == texthandle) {
        if (ds_map_find_value(async_load, "status") == 0) {
            result = string(ds_map_find_value(async_load, "result"))
            if (result == "IOException" || result == "") {
                timeout = 0
                check_update = "Error: Failed to retrieve result..."
                result = ""
                scr_cw_prompt(check_update, 0, "", "", "", "Y", 1)
                prompt_n = 1
                connect = 0
            }
            else {
                timeout = 0
                chao_vt = ""
                get_u = real(string_separate_beg(result, "["))
                Get_download_link = string_separate_mid(result, "[", "]")
                mod_ver = string_separate_mid(result, "]", "{")
                manual_limit = real(string_separate_end(result, "{"))
                show_debug_message(string(get_u))
                show_debug_message(Get_download_link)
                show_debug_message(string(mod_ver))
                show_debug_message(string(manual_limit))
                if (string_char_at(Get_download_link, 2) == "G") {
                    if (update_check == 0) {
                        alarm[1] = -1
                        scr_cw_prompt("Daily server maintenance is currently running.#Please try again later.", 0, "", "", "", "Y", 0)
                        connect = 0
                        result = ""
                        check_version = 0
                    }
                    else
                        instance_destroy()
                }
                else if (get_u == 0 || Get_download_link == "" || manual_limit == 0) {
                    alarm[1] = 30
                    result = ""
                }
                else if (manual_limit > get_chao_version && global.os_config == "windows") {
                    chao_vt = string(get_u)
                    chao_vt = string_insert(".", chao_vt, 2)
                    chao_vt = string_insert(".", chao_vt, 4)
                    prompt_n = 3
                    connect = 0
                    scr_cw_prompt((("There's an update available.#But it requires a manual download.##Visit the download page for Update v" + chao_vt) + "?"), 2, "Yes", "No", "", "Y", 1)
                    texthandle = -1
                    connect = 0
                    alarm[1] = -1
                    result = ""
                    check_version = 0
                    select_update = 1
                }
                else if (get_u > get_chao_version) {
                    chao_vt = string(get_u)
                    chao_vt = string_insert(".", chao_vt, 2)
                    chao_vt = string_insert(".", chao_vt, 4)
                    prompt_n = 2
                    connect = 0
                    if (global.os_config == "windows")
                        scr_cw_prompt((("There's an update available for download.#Would you like to download v" + chao_vt) + "?##Your save data will not be affected!"), 2, "Yes", "No", "", "Y", 1)
                    else {
                        scr_cw_prompt("There's an update available for download.#Would you like to visit the download page?", 2, "Yes", "No", "", "Y", 1)
                        prompt_n = 3
                    }
                    texthandle = -1
                    connect = 0
                    alarm[1] = -1
                    result = ""
                    check_version = 0
                    select_update = 1
                }
                else if (real(mod_ver) > global.x_version_int) {
                  chao_vt = mod_ver
                  chao_vt = string_insert(".", chao_vt, 2)
                  chao_vt = string_insert(".", chao_vt, 4)
                  prompt_n = 2
                  connect = 0
                  if (global.os_config == "windows")
                      scr_cw_prompt((("There's an update available for download.#Would you like to download#(MOD UPDATE) v" + chao_vt) + "?##Your save data will not be affected!"), 2, "Yes", "No", "", "Y", 1)
                  else {
                      scr_cw_prompt("There's an update available for download.#Would you like to visit the download page?", 2, "Yes", "No", "", "Y", 1)
                      prompt_n = 3
                  }
                  texthandle = -1
                  connect = 0
                  alarm[1] = -1
                  result = ""
                  check_version = 0
                  select_update = 1
                }
                else if (update_check == 0) {
                    alarm[1] = -1
                    scr_cw_prompt("Your current version is up to date.", 0, "", "", "", "Y", 0)
                    connect = 0
                    result = ""
                    check_version = 0
                }
                else
                    instance_destroy()
            }
        }
        else if (ds_map_find_value(async_load, "status") == -1) {
            alarm[1] = 30
            result = ""
        }
    }
    if (ds_map_find_value(async_load, "id") == file_handle) {
        status = ds_map_find_value(async_load, "status")
        if (status == 0) {
            path = ds_map_find_value(async_load, "result")
            files = zip_unzip(path, gm_path_input)
            if (files > 0) {
                show_debug_message("Download and unzip success!")
                alarm[3] = 2
            }
        }
        else if (status == 1) {
            timeout = 0
            file_progress = ds_map_find_value(async_load, "sizeDownloaded")
            alarm[2] = -1
        }
        else if (status == -1) {
            timeout = 0
            check_update = "Lost connection to files..."
            result = ""
            scr_cw_prompt(check_update, 0, "", "", "", "Y", 1)
            prompt_n = 1
            connect = 0
        }
    }
}
