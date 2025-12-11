package com.prosheep.ProjectBackend.unityapi.service;

import com.baomidou.mybatisplus.core.conditions.query.QueryWrapper;
import com.baomidou.mybatisplus.core.conditions.update.UpdateWrapper;
import com.prosheep.ProjectBackend.unityapi.entity.Dict;
import com.prosheep.ProjectBackend.unityapi.mapper.DictMapper;
import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class DictService {

    private final DictMapper dictMapper;

    public DictService(DictMapper dictMapper) {
        this.dictMapper = dictMapper;
    }

    public List<Dict> getAllLanguageDict(String dictLanguage) {
        return dictMapper.selectList(
                new QueryWrapper<Dict>()
                        .eq("dict_type", "language")
                        .eq("dict_language", dictLanguage)
        );
    }

    // Bulk add
    public void addBulk(List<Dict> dicts) {
        for (Dict dict : dicts) {
            dictMapper.insert(dict);
        }
    }

    // Bulk update
    public void updateBulk(List<Dict> dicts) {
        for (Dict dict : dicts) {
            UpdateWrapper<Dict> wrapper = new UpdateWrapper<>();
            wrapper.eq("dict_value", dict.getDict_value())
                    .eq("dict_language", dict.getDict_language())
                    .eq("dict_type", dict.getDict_type());
            dictMapper.update(dict, wrapper);
        }
    }
}
