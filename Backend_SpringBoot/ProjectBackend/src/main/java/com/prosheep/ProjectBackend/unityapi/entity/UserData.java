package com.prosheep.ProjectBackend.unityapi.entity;

import com.baomidou.mybatisplus.annotation.TableId;
import com.baomidou.mybatisplus.annotation.IdType;
import com.baomidou.mybatisplus.annotation.TableName;
import com.baomidou.mybatisplus.annotation.TableField;

import java.time.LocalDateTime;
@TableName("bright_user_data")
public class UserData {

    @TableId(value = "id", type = IdType.AUTO)
    private Long id;              // 用户ID

    private String username;      // 用户账号 (唯一)

    @TableField("unlock_status")
    private String unlock_status;  // 课程解锁状态 (JSON存储，建议用String接收)

    private Long deptid;          // 部门ID

    @TableField("create_by")
    private String create_by;      // 创建者

    @TableField("create_time")
    private LocalDateTime create_time;  // 创建时间

    @TableField("update_by")
    private String update_by;      // 更新者

    @TableField("update_time")
    private LocalDateTime update_time;  // 更新时间

    // --- Getters and Setters ---
    public Long getId() {
        return id;
    }
    public void setId(Long id) {
        this.id = id;
    }

    public String getUsername() {
        return username;
    }
    public void setUsername(String username) {
        this.username = username;
    }

    public String getUnlock_status() {
        return unlock_status;
    }
    public void setUnlock_status(String unlock_status) {
        this.unlock_status = unlock_status;
    }

    public Long getDeptid() {
        return deptid;
    }
    public void setDeptid(Long deptid) {
        this.deptid = deptid;
    }

    public String getCreate_by() {
        return create_by;
    }
    public void setCreate_by(String create_by) {
        this.create_by = create_by;
    }

    public LocalDateTime getCreate_time() {
        return create_time;
    }
    public void setCreate_time(LocalDateTime create_time) {
        this.create_time = create_time;
    }

    public String getUpdate_by() {
        return update_by;
    }
    public void setUpdate_by(String update_by) {
        this.update_by = update_by;
    }

    public LocalDateTime getUpdate_time() {
        return update_time;
    }
    public void setUpdate_time(LocalDateTime update_time) {
        this.update_time = update_time;
    }

}