package com.ort.studyup.repositories

import com.ort.studyup.common.models.Deck
import com.ort.studyup.services.DeckService
import com.ort.studyup.services.check

class DeckRepository(
    private val deckService: DeckService,
) {

    suspend fun decksFromUser(id:Int): List<Deck> {
        return deckService.decksFromUser(id).check()
    }

}