package com.ort.studyup.services

import com.ort.studyup.common.models.Deck
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST

interface DeckService {

    //TODO
    @GET("TBD")
    suspend fun decksFromUser(id:Int): Response<List<Deck>>


}