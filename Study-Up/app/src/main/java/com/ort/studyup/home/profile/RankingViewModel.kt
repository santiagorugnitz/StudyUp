package com.ort.studyup.home.profile

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.User
import com.ort.studyup.common.renderers.ResultItemRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.GroupRepository
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceError

class RankingViewModel(
    private val userRepository: UserRepository
) : BaseViewModel() {

    fun loadRanking(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            val user = userRepository.getUser()
            val ranking = userRepository.ranking()
            result.postValue(ranking.mapIndexed { pos, it ->
                ResultItemRenderer.Item(
                    pos,
                    it.username,
                    it.score,
                    it.username == user?.username
                )
            })
        }
        return result
    }

}