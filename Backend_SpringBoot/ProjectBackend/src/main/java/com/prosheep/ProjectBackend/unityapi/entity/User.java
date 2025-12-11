package com.prosheep.ProjectBackend.unityapi.entity;

import com.baomidou.mybatisplus.annotation.TableId;
import com.baomidou.mybatisplus.annotation.TableName;
import com.baomidou.mybatisplus.annotation.IdType;
import lombok.Data;

import java.time.LocalDateTime;

@Data
@TableName("bright_user")
public class User {

    @TableId(value = "user_id", type = IdType.AUTO)
    private Long user_id;   // 用户ID

    private String username;   // 用户账号
    private String nickname;   // 用户昵称
    private Integer usertype;  // 用户类型（0系统用户）
    private Long deptid;       // 部门ID
    private String phonenumber; // 手机号码
    private String sex;        // 用户性别（0男 1女）
    private String avatar;     // 头像地址
    private String password;   // 密码
    private String status;     // 帐号状态（0正常 1停用）
    private String del_flag;    // 删除标志（0代表存在 1代表删除）
    private String login_ip;    // 最后登录IP
    private LocalDateTime login_date; // 最后登录时间
    private String create_by;   // 创建者
    private LocalDateTime create_time; // 创建时间
    private String update_by;   // 更新者
    private LocalDateTime update_time; // 更新时间
    private String remark;     // 备注
}
