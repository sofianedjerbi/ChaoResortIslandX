var chao_v, chao_vt, get_u, status, path, files, mod_ver;
if (ds_map_find_value(async_load, "id") == texthandle) {
    if (ds_map_find_value(async_load, "status") == 0) {
        result = string(ds_map_find_value(async_load, "result"))
        if (result == "IOException" || result == "") {
            check_update = "Error: Failed to retrieve result..."
            result = ""
            connect = 0
            cancel_con = 1
        }
        else {
            chao_v = ""
            chao_vt = ""
            get_u = real(string_separate_beg(result, "["))
            Get_download_link = string_separate_mid(result, "[", "]")
            mod_ver = real(string_separate_mid(result, "]", "{"))
            show_debug_message(result)
            show_debug_message(string(get_u))
            show_debug_message(Get_download_link)
            show_debug_message(string(mod_ver))
            if (get_u == 0 || Get_download_link == "") {
                alarm[1] = 30
                result = ""
            }
            else if (get_u > get_chao_version) {
                chao_v = string(get_chao_version)
                chao_v = string_insert(".", chao_v, 2)
                chao_v = string_insert(".", chao_v, 4)
                chao_vt = string(get_u)
                chao_vt = string_insert(".", chao_vt, 2)
                chao_vt = string_insert(".", chao_vt, 4)
                check_update = ((("Yeah, there's an update.##Current version: " + chao_v) + "#Latest version: ") + chao_vt)
                connect = 0
                cancel_con = 1
                alarm[1] = -1
                result = ""
                check_version = 0
                select_update = 1
            }
            else if (mod_ver > global.x_version_int) {
                xchao_v = string(mod_ver)
                xchao_v = string_insert(".", chao_v, 2)
                xchao_v = string_insert(".", chao_v, 4)
                check_update = ((("Yeah, there's a mod update.##Current version: " + global.x_version) + "#Latest version: ") + xchao_v)
                connect = 0
                cancel_con = 1
                alarm[1] = -1
                result = ""
                check_version = 0
                select_update = 1
            }
            else {
                chao_v = string(get_chao_version)
                chao_v = string_insert(".", chao_v, 2)
                chao_v = string_insert(".", chao_v, 4)
                chao_vt = string(get_u)
                chao_vt = string_insert(".", chao_vt, 2)
                chao_vt = string_insert(".", chao_vt, 4)
                alarm[1] = -1
                check_update = ((("Nah, you're up-to-date.##Current version: " + chao_v) + "#Latest version: ") + chao_vt)
                result = ""
                check_version = 0
            }
        }
    }
}
if (ds_map_find_value(async_load, "id") == file_handle) {
    status = ds_map_find_value(async_load, "status")
    show_debug_message(((("Received file! " + string(status)) + " :: ") + string(current_time)))
    if (status == 0) {
        path = ds_map_find_value(async_load, "result")
        files = zip_unzip(path, gm_path_input)
        if (files > 0) {
            show_debug_message("Download and unzip success!")
            alarm[3] = 2
        }
    }
    else if (status == 1)
        file_progress = ds_map_find_value(async_load, "sizeDownloaded")
    else if (status == -1) {
        alarm[0] = 30
        show_debug_message("Failed to check for downloads.")
    }
}
