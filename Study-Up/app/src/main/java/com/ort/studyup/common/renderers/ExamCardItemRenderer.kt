package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_examcard.view.*

class ExamCardItemRenderer(private val callback: Callback) : ViewRenderer<ExamCardItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_examcard, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.question.text = model.question
        //TODO:
        view.editButton.setOnClickListener {
            callback.onEditExamCard(model.id, model.question, model.answer)
        }
    }

    class Item(
        val id: Int,
        val question: String,
        val answer: Boolean,
    )

    interface Callback {
        fun onEditExamCard(id: Int, question: String, answer: Boolean)
    }
}
