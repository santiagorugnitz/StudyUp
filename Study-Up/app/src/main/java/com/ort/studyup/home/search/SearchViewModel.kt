package com.ort.studyup.home.search

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.renderers.GroupSearchResultRenderer
import com.ort.studyup.common.renderers.UserSearchResultRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.GroupRepository
import com.ort.studyup.repositories.UserRepository

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
            items.addAll(
                if (searchUsers) {
                    val results = userRepository.searchUser(query)
                    results.map {
                        UserSearchResultRenderer.Item(
                            it.username,
                            it.following
                        )
                    }
                } else {
                    val results = groupRepository.searchGroup(query)
                    results.map {
                        GroupSearchResultRenderer.Item(
                            it.id,
                            it.name,
                            it.teacherName,
                            it.subscribed
                        )
                    }
                }
            )
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

    fun onSubChange(position: Int): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            (items[position] as GroupSearchResultRenderer.Item).let {
                if (it.subscribed) {
                    groupRepository.unsubscribe(it.id)
                    it.subscribed = false
                } else {
                    groupRepository.subscribe(it.id)
                    it.subscribed = true
                }
                result.postValue(true)
            }
        }
        return result
    }

}