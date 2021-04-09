package com.ort.studyup.common.models

class Deck(
    val id:Int,
    val name:String,
    val difficulty:Int,
    val subject:String,
    val flashcards: List<Any>?,
    val isHidden:Boolean,
)