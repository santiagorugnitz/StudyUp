package com.ort.studyup.services

import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.NewDeckRequest
import com.ort.studyup.common.models.NewGroupRequest
import retrofit2.Response
import retrofit2.http.*

interface GroupService {


    @POST("api/groups")
    suspend fun createGroup(@Body data: NewGroupRequest): Response<*>

}