package com.ort.studyup.home.search

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.R
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.User
import com.ort.studyup.common.renderers.UserSearchResultRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.repositories.GroupRepository
import com.ort.studyup.repositories.UserRepository
import com.ort.studyup.services.ServiceError

class SearchViewModel(
    private val userRepository: UserRepository,
    private val groupRepository: GroupRepository,
) : BaseViewModel() {

    var searchUsers = true
    var items = mutableListOf<Any>()

    fun search(query: String): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()
        executeService {
            items.clear()
            items.addAll(if (searchUsers) userRepository.searchUser(query) else groupRepository.searchGroup(query))
            result.postValue(items)
        }
        return result
    }

    fun onFollowChange(position: Int): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            (items[position] as UserSearchResultRenderer.Item).let {
                if (it.following) {
                    userRepository.unfollow(it.username)
                    it.following = false
                } else {
                    userRepository.follow(it.username)
                    it.following = true
                }
                result.postValue(true)
            }
        }
        return result
    }

}