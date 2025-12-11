package com.prosheep.ProjectBackend.unityapi.controller;

import com.prosheep.ProjectBackend.unityapi.entity.R;
import com.prosheep.ProjectBackend.unityapi.entity.UserData;
import com.prosheep.ProjectBackend.unityapi.mapper.UserDataMapper;
import com.prosheep.ProjectBackend.unityapi.service.UserDataService;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/v1/userdata")
public class UserDataController {


    private final UserDataMapper userDataMapper;
    private final UserDataService userDataService;


    @GetMapping
    public R<List<UserData>> getAll() {
        var list = userDataMapper.selectList(null);
        return R.ok("Fetched all users successfully.", list);
    }

    @GetMapping("/id/{id}")
    public R<UserData> getById(@PathVariable Long id) {
        var result = userDataMapper.selectById(id);
        return result != null
                ? R.ok("Fetched user successfully.", result)
                : R.fail("User not found.");
    }

    @PostMapping
    public R<UserData> create(@RequestBody UserData userData) {
        int rows = userDataMapper.insert(userData);
        return rows > 0
                ? R.ok("Created user successfully.", userData)
                : R.fail("Failed to create user.");
    }

    @PutMapping("/id/{id}")
    public R<UserData> update(@PathVariable Long id, @RequestBody UserData userData) {
        userData.setId(id);
        int rows = userDataMapper.updateById(userData);
        return rows > 0
                ? R.ok("Updated user successfully.", userData)
                : R.fail("Failed to update user.");
    }

    @DeleteMapping("/id/{id}")
    public R<Long> delete(@PathVariable Long id) {
        int rows = userDataMapper.deleteById(id);
        return rows > 0
                ? R.ok("Deleted user successfully.", id)
                : R.fail("Failed to delete user.");
    }

    @GetMapping("/username/{username}")
    public R<UserData> getByUsername(@PathVariable String username) {
        var result = userDataMapper.selectByUsername(username);
        return result != null
                ? R.ok("Operation Successfully!", result)
                : R.fail("User not found.");
    }

    @PutMapping("/username/{username}")
    public R<UserData> updateByUsername(@PathVariable String username,
                                        @RequestBody UserData userData) {
        userData.setUsername(username);
        int rows = userDataMapper.updateByUsername(userData);
        return rows > 0
                ? R.ok("Updated user successfully.", userData)
                : R.fail("Failed to update user.");
    }

    @DeleteMapping("/username/{username}")
    public R<String> deleteByUsername(@PathVariable String username) {
        int rows = userDataMapper.deleteByUsername(username);
        return rows > 0
                ? R.ok("Deleted user successfully.", username)
                : R.fail("Failed to delete user.");
    }
}