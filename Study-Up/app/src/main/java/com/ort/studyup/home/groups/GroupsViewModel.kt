package com.ort.studyup.home.groups

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import com.ort.studyup.common.models.DeckItem
import com.ort.studyup.common.renderers.AssignDeckItemRenderer
import com.ort.studyup.common.renderers.DeletableDeckItemRenderer
import com.ort.studyup.common.ui.BaseViewModel
import com.ort.studyup.repositories.DeckRepository
import com.ort.studyup.repositories.GroupRepository
import com.ort.studyup.repositories.UserRepository

class GroupsViewModel(
    private val groupRepository: GroupRepository,
    private val deckRepository: DeckRepository,
    private val userRepository: UserRepository
) : BaseViewModel() {

    private val items = mutableListOf<Any>()

    fun loadGroups(): LiveData<List<Any>> {
        val result = MutableLiveData<List<Any>>()

        executeService {
            val user = userRepository.getUser()
            user?.let {
                val decks = deckRepository.decksFromUser(user.id).map { DeckItem(it.id, it.name) }
                val response = groupRepository.groups()
                items.clear()
                response.forEach { group ->
                    items.add(
                        AssignDeckItemRenderer.Item(
                            group.id,
                            group.name,
                            decks.filter { !group.decks.contains(it) }
                        )
                    )
                    group.decks.forEach {
                        items.add(
                            DeletableDeckItemRenderer.Item(
                                it.id,
                                it.name,
                                group.id
                            )
                        )
                    }
                }
                result.postValue(items)
            }

        }
        return result
    }

    fun assignDeck(groupId: Int, deckId: Int): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            groupRepository.assignDeck(groupId, deckId)
            result.postValue(true)
        }
        return result
    }

    fun unassignDeck(groupId: Int, deckId: Int): LiveData<Boolean> {
        val result = MutableLiveData<Boolean>()
        executeService {
            groupRepository.unassignDeck(groupId, deckId)
            result.postValue(true)
        }
        return result
    }

}