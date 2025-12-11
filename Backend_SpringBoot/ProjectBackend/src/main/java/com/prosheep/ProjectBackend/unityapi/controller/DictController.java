package com.prosheep.ProjectBackend.unityapi.controller;

import com.prosheep.ProjectBackend.unityapi.entity.Dict;
import com.prosheep.ProjectBackend.unityapi.entity.R;
import com.prosheep.ProjectBackend.unityapi.service.DictService;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/v1/dict")
public class DictController {

    private final DictService dictData;

    @GetMapping("/language/{lang}")
    public R<List<Dict>> getAllLanguageDict(@PathVariable("lang") String lang) {
        return R.ok("Operation Succeed! ", dictData.getAllLanguageDict(lang));
    }

    @PostMapping("/add")
    public R addBulk(@RequestBody List<Dict> dicts) {
        dictData.addBulk(dicts);
        return R.ok("Bulk add successful");
    }

    @PutMapping("/update")
    public R updateBulk(@RequestBody List<Dict> dicts) {
        dictData.updateBulk(dicts);
        return R.ok("Bulk update successful");
    }
}
