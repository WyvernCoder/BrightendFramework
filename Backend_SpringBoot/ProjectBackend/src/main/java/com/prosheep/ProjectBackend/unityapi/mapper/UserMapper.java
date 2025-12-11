package com.prosheep.ProjectBackend.unityapi.mapper;

import com.prosheep.ProjectBackend.unityapi.entity.User;
import org.apache.ibatis.annotations.*;

@Mapper
public interface UserMapper {

    @Select("SELECT * FROM bright_user WHERE user_id = #{userId}")
    User selectById(Long userId);

    @Select("SELECT * FROM bright_user")
    java.util.List<User> selectAll();

    @Insert("INSERT INTO bright_user(username, nickname, usertype, deptid, phonenumber, sex, avatar, " +
            "password, status, del_flag, login_ip, login_date, create_by, create_time, update_by, update_time, remark) " +
            "VALUES(#{username}, #{nickname}, #{usertype}, #{deptid}, #{phonenumber}, #{sex}, #{avatar}, " +
            "#{password}, #{status}, #{delFlag}, #{loginIp}, #{loginDate}, #{createBy}, #{createTime}, #{updateBy}, #{updateTime}, #{remark})")
    @Options(useGeneratedKeys = true, keyProperty = "userId")
    int insert(User user);

    @Update("UPDATE bright_user SET username=#{username}, nickname=#{nickname}, usertype=#{usertype}, deptid=#{deptid}, " +
            "phonenumber=#{phonenumber}, sex=#{sex}, avatar=#{avatar}, password=#{password}, status=#{status}, " +
            "del_flag=#{delFlag}, login_ip=#{loginIp}, login_date=#{loginDate}, create_by=#{createBy}, create_time=#{createTime}, " +
            "update_by=#{updateBy}, update_time=#{updateTime}, remark=#{remark} WHERE user_id=#{userId}")
    int update(User user);

    @Delete("DELETE FROM bright_user WHERE user_id = #{userId}")
    int deleteById(Long userId);

    // 🔑 New function 1: select by username & password, return full user
    @Select("SELECT * FROM bright_user WHERE username = #{username} AND password = #{password}")
    User selectByUsernameAndPassword(@Param("username") String username,
                                     @Param("password") String password);

    // 🔑 New function 2: select by username, return only password
    @Select("SELECT password FROM bright_user WHERE username = #{username}")
    String selectPasswordByUsername(@Param("username") String username);

    // Update by username
    @Update("UPDATE bright_user SET nickname=#{nickname}, usertype=#{usertype}, deptid=#{deptid}, " +
            "phonenumber=#{phonenumber}, sex=#{sex}, avatar=#{avatar}, password=#{password}, status=#{status}, " +
            "del_flag=#{delFlag}, login_ip=#{loginIp}, login_date=#{loginDate}, create_by=#{createBy}, create_time=#{createTime}, " +
            "update_by=#{updateBy}, update_time=#{updateTime}, remark=#{remark} WHERE username=#{username}")
    int updateByUsername(User user);

    // Delete by username
    @Delete("DELETE FROM bright_user WHERE username = #{username}")
    int deleteByUsername(@Param("username") String username);

    // Select by username
    @Select("SELECT * FROM bright_user WHERE username = #{username}")
    User selectByUsername(@Param("username") String username);

}