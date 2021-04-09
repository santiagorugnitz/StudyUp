package com.ort.studyup.home

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.ui.BaseViewModel

class DecksViewModel : BaseViewModel() {
    fun loadDecks(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        //TODO: load items
        return result

    }
}