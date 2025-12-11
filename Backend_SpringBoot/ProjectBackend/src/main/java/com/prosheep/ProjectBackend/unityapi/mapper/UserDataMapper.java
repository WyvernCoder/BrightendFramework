package com.prosheep.ProjectBackend.unityapi.mapper;

import com.baomidou.mybatisplus.core.mapper.BaseMapper;
import com.prosheep.ProjectBackend.unityapi.entity.UserData;
import org.apache.ibatis.annotations.*;

@Mapper
public interface UserDataMapper extends BaseMapper<UserData> {

    // 🔎 Get user data by username
    @Select("SELECT * FROM bright_user_data WHERE username = #{username}")
    UserData selectByUsername(@Param("username") String username);

    // ✏️ Update user data by username
    @Update("UPDATE bright_user_data SET " +
            "unlock_status = #{unlockStatus}, " +
            "deptid = #{deptid}, " +
            "create_by = #{createBy}, " +
            "create_time = #{createTime}, " +
            "update_by = #{updateBy}, " +
            "update_time = #{updateTime} " +
            "WHERE username = #{username}")
    int updateByUsername(UserData userData);

    // 🗑️ Delete user data by username
    @Delete("DELETE FROM bright_user_data WHERE username = #{username}")
    int deleteByUsername(@Param("username") String username);
}