package com.prosheep.ProjectBackend.unityapi.controller;

import com.prosheep.ProjectBackend.unityapi.entity.Level;
import com.prosheep.ProjectBackend.unityapi.entity.R;
import com.prosheep.ProjectBackend.unityapi.mapper.LevelMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/v1/levels")
public class LevelController {

    private final LevelMapper levelMapper;


    @GetMapping
    public R<List<Level>> getAll() {
        return R.ok("Fetched all levels successfully.", levelMapper.selectAll());
    }

    @GetMapping("/uid/{uid}")
    public R<Level> getByUid(@PathVariable String uid) {
        var level = levelMapper.selectByUid(uid);
        return level != null ? R.ok("Fetched level successfully.", level)
                : R.fail("Level not found.");
    }

    @PostMapping
    public R<Level> add(@RequestBody Level level) {
        int rows = levelMapper.add(level);
        return rows > 0 ? R.ok("Level created successfully.", level)
                : R.fail("Failed to create level.");
    }

    @PutMapping("/uid/{uid}")
    public R<Level> edit(@PathVariable String uid, @RequestBody Level level) {
        level.setUid(uid);
        int rows = levelMapper.edit(level);
        return rows > 0 ? R.ok("Level updated successfully.", level)
                : R.fail("Failed to update level.");
    }

    @DeleteMapping("/uid/{uid}")
    public R<String> delete(@PathVariable String uid) {
        int rows = levelMapper.deleteByUid(uid);
        return rows > 0 ? R.ok("Level deleted successfully.", uid)
                : R.fail("Failed to delete level.");
    }
}
