package com.prosheep.ProjectBackend.unityapi.entity;

import com.baomidou.mybatisplus.annotation.TableId;
import com.baomidou.mybatisplus.annotation.TableName;
import lombok.Data;

@Data
@TableName("bright_dict_data")
public class Dict {

    @TableId
    private Long dict_code;

    private String dict_label;
    private String dict_value;
    private String dict_type;
    private String dict_language;
    private String length_list;
    private String status;
    private Long deptid;
    private String create_by;
    private java.time.LocalDateTime create_time;
    private String update_by;
    private java.time.LocalDateTime update_time;
}
