package com.prosheep.ProjectBackend.unityapi.service;

import com.prosheep.ProjectBackend.unityapi.entity.User;
import com.prosheep.ProjectBackend.unityapi.mapper.UserMapper;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UserService {

    private final UserMapper userMapper;

    public UserService(UserMapper userMapper) {
        this.userMapper = userMapper;
    }

    /**
     * 验证登录密码
     */
    public boolean validateUser(String username, String password) {
        String storedPassword = userMapper.selectPasswordByUsername(username);
        return storedPassword != null && storedPassword.equals(password);
    }

    /**
     * 返回全部用户数据
     * 若失败，则返回null
     */
    public User getUserData(String username, String password) {
        return userMapper.selectByUsernameAndPassword(username, password);
    }

    /**
     * 获取全表用户数据
     */
    public List<User> getAllUsers() {
        return userMapper.selectAll();
    }

    public int addUser(User user) {
        return userMapper.insert(user);
    }

    public int updateUserByUsername(User user) {
        return userMapper.updateByUsername(user);
    }

    public int deleteUserByUsername(String username) {
        return userMapper.deleteByUsername(username);
    }

    public User getUserByUsername(String username) {
        return userMapper.selectByUsername(username);
    }

}
