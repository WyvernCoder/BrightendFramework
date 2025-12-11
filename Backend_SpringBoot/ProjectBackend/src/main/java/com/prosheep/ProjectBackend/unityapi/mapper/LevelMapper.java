package com.prosheep.ProjectBackend.unityapi.mapper;

import com.baomidou.mybatisplus.core.mapper.BaseMapper;
import com.prosheep.ProjectBackend.unityapi.entity.Level;
import org.apache.ibatis.annotations.*;

import java.util.List;

@Mapper
public interface LevelMapper extends BaseMapper<Level> {

    @Select("SELECT * FROM bright_level_list WHERE uid = #{uid}")
    Level selectByUid(@Param("uid") String uid);

    @Insert("INSERT INTO bright_level_list(uid, cover, `json`, label, `describe`, category_id, default_unlock, max_stars, create_by, update_by) " +
            "VALUES(#{uid}, #{cover}, #{json}, #{label}, #{describe}, #{categoryId}, #{defaultUnlock}, #{maxStars}, #{createBy}, #{updateBy})")
    int add(Level level);

    @Update("UPDATE bright_level_list SET cover=#{cover}, `json`=#{json}, label=#{label}, `describe`=#{describe}, category_id=#{categoryId}, " +
            "default_unlock=#{defaultUnlock}, max_stars=#{maxStars}, update_by=#{updateBy}, update_time=NOW() WHERE uid=#{uid}")
    int edit(Level level);

    @Delete("DELETE FROM bright_level_list WHERE uid=#{uid}")
    int deleteByUid(@Param("uid") String uid);

    @Select("SELECT * FROM bright_level_list")
    List<Level> selectAll();
}
