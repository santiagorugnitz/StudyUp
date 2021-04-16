package com.ort.studyup.repositories

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

}