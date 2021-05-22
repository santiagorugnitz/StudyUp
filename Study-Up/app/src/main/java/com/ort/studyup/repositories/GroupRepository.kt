package com.ort.studyup.repositories

import com.ort.studyup.common.models.Group
import com.ort.studyup.common.models.NewGroupRequest
import com.ort.studyup.services.GroupService
import com.ort.studyup.services.check

class GroupRepository(
    private val groupService: GroupService,
) {

    suspend fun createGroup(name: String) {
        groupService.createGroup(
            NewGroupRequest(name)
        ).check()
    }

    suspend fun searchGroup(name: String) = groupService.searchGroup(name).check()

    suspend fun subscribe(id: Int) {
        groupService.subscribe(id).check()
    }

    suspend fun unsubscribe(id: Int) {
        groupService.unsubscribe(id).check()
    }

    suspend fun assignDeck(groupId:Int,deckId:Int){
        groupService.assign(groupId, deckId)
    }

    suspend fun unassignDeck(groupId:Int,deckId:Int){
        groupService.unassign(groupId, deckId)
    }


    suspend fun groups():List<Group>{
        return groupService.groups().check()
    }


}