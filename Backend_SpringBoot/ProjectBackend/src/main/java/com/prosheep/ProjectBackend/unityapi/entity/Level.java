package com.prosheep.ProjectBackend.unityapi.entity;

import com.baomidou.mybatisplus.annotation.IdType;
import com.baomidou.mybatisplus.annotation.TableId;
import com.baomidou.mybatisplus.annotation.TableName;
import com.baomidou.mybatisplus.annotation.TableField;

import java.time.LocalDateTime;

@TableName("bright_level_list")
public class Level {

    @TableId(value = "id", type = IdType.AUTO)
    private Long id;

    private String uid;            // 关卡32位UID
    private String cover;          // 封面

    @TableField("json")
    private String json;           // 关卡数据 (stored as JSON string)

    private String label;          // 关卡名称locales
    private String describe;       // 描述
    private Integer category_id;    // 分类编号
    private Integer default_unlock; // 是否默认解锁
    private Integer max_stars;      // 最大星星数
    private String create_by;       // 创建者
    private LocalDateTime create_time; // 创建时间
    private String update_by;       // 更新者
    private LocalDateTime update_time; // 更新时间

    // --- Getters & Setters ---
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }

    public String getUid() { return uid; }
    public void setUid(String uid) { this.uid = uid; }

    public String getCover() { return cover; }
    public void setCover(String cover) { this.cover = cover; }

    public String getJson() { return json; }
    public void setJson(String json) { this.json = json; }

    public String getLabel() { return label; }
    public void setLabel(String label) { this.label = label; }

    public String getDescribe() { return describe; }
    public void setDescribe(String describe) { this.describe = describe; }

    public Integer getCategory_id() { return category_id; }
    public void setCategory_id(Integer category_id) { this.category_id = category_id; }

    public Integer getDefault_unlock() { return default_unlock; }
    public void setDefault_unlock(Integer default_unlock) { this.default_unlock = default_unlock; }

    public Integer getMax_stars() { return max_stars; }
    public void setMax_stars(Integer max_stars) { this.max_stars = max_stars; }

    public String getCreate_by() { return create_by; }
    public void setCreate_by(String create_by) { this.create_by = create_by; }

    public LocalDateTime getCreate_time() { return create_time; }
    public void setCreate_time(LocalDateTime create_time) { this.create_time = create_time; }

    public String getUpdate_by() { return update_by; }
    public void setUpdate_by(String update_by) { this.update_by = update_by; }

    public LocalDateTime getUpdate_time() { return update_time; }
    public void setUpdate_time(LocalDateTime update_time) { this.update_time = update_time; }
}
