package com.ort.studyup.home.profile

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.renderers.ResultItemRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.UserRepository

class RankingViewModel(
    private val userRepository: UserRepository
) : BaseViewModel() {

    fun loadRanking(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            val user = userRepository.getUser()
            val ranking = userRepository.ranking()

            result.postValue(ranking.sortedByDescending { it.score }.mapIndexed { pos, it ->
                ResultItemRenderer.Item(
                    pos+1,
                    it.username,
                    it.score,
                    it.username == user?.username
                )
            })
        }
        return result
    }

}