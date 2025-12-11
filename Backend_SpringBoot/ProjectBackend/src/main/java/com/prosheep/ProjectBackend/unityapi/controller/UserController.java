package com.prosheep.ProjectBackend.unityapi.controller;

import com.prosheep.ProjectBackend.unityapi.entity.R;
import com.prosheep.ProjectBackend.unityapi.entity.User;
import com.prosheep.ProjectBackend.unityapi.service.UserService;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("v1/user")
public class UserController {

    private final UserService userService;

    /**
     * validate username & password.
     */
    @PostMapping("/validate")
    public R<Boolean> validate(@RequestParam String username,
                            @RequestParam String password) {
        boolean valid = userService.validateUser(username, password);
        return valid
                ? R.ok("Check successful.", true)
                : R.fail("Invalid username or password.", false);
    }

    /**
     * get user with username & password.
     */
    @PostMapping("/login")
    public R<User> login(@RequestParam String username,
                            @RequestParam String password) {
        boolean valid = userService.validateUser(username, password);
        return valid
                ? R.ok("Login successful.", userService.getUserByUsername(username))
                : R.fail("Invalid username or password. ");
    }

    /**
     * Get all users (admin use case).
     */
    @GetMapping
    public R<java.util.List<User>> getAllUsers() {
        var list = userService.getAllUsers();
        return R.ok("Fetched all users successfully.", list);
    }

    /**
     * Add a new user.
     */
    @PostMapping("/add")
    public R<Boolean> addUser(@RequestBody User user) {
        int rows = userService.addUser(user);
        return rows > 0
                ? R.ok("User added successfully.", true)
                : R.fail("Failed to add user.");
    }

    /**
     * Update user by username.
     */
    @PutMapping("/update")
    public R<Boolean> updateUser(@RequestBody User user) {
        int rows = userService.updateUserByUsername(user);
        return rows > 0
                ? R.ok("User updated successfully.", true)
                : R.fail("Failed to update user.");
    }

    /**
     * Delete user by username.
     */
    @DeleteMapping("/delete/{username}")
    public R<Boolean> deleteUser(@PathVariable String username) {
        int rows = userService.deleteUserByUsername(username);
        return rows > 0
                ? R.ok("User deleted successfully.", true)
                : R.fail("Failed to delete user.");
    }

    @GetMapping("/username/{username}")
    public R<User> getByUsername(@PathVariable String username) {
        var result = userService.getUserByUsername(username);
        return result != null
                ? R.ok("Operation Successfully!", result)
                : R.fail("User not found.");
    }

}