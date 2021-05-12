package com.ort.studyup.common.renderers

import android.icu.number.Scale.none
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import com.ort.studyup.R
import com.ort.studyup.common.models.ExamItem
import com.ort.studyup.common.models.Group
import com.ort.studyup.common.models.GroupItem
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_assign_deck.view.*
import kotlinx.android.synthetic.main.item_exam.view.*
import kotlinx.android.synthetic.main.item_exam.view.spinner

class ExamItemRenderer(private val callback: Callback) : ViewRenderer<ExamItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_exam, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.examName.text = model.name
        if (model.groupName.isNullOrEmpty()) {
            view.group.visibility = View.GONE
            view.spinner.visibility = View.VISIBLE
            ArrayAdapter(
                    view.context,
                    android.R.layout.simple_spinner_item,
                    mutableListOf(view.context.getString(R.string.none)).apply { addAll(model.groups.map { it.name }) }.toTypedArray()
            ).also {
                it.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                view.spinner.adapter = it
            }
            view.spinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
                override fun onItemSelected(p0: AdapterView<*>?, p1: View?, position: Int, p3: Long) {
                    if (position != 0)
                        callback.onAssignExam(model.id, model.groups[position - 1].id)
                }

                override fun onNothingSelected(p0: AdapterView<*>?) {
                }

            }

        } else {
            view.group.text = model.groupName
            view.group.visibility = View.VISIBLE
            view.spinner.visibility = View.GONE
        }
        view.setOnClickListener {
            callback.onExamClicked(ExamItem(model.id, model.name, model.groupName))
        }
    }

    class Item(
            val id: Int,
            val name: String,
            val groupName: String?,
            val groups: List<GroupItem> = listOf()
    )

    interface Callback {
        fun onExamClicked(exam: ExamItem)
        fun onAssignExam(examId: Int, groupId: Int)
    }
}