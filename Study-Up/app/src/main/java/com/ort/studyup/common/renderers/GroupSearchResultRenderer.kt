package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_group_search_result.view.*

class GroupSearchResultRenderer(private val callback: Callback) : ViewRenderer<GroupSearchResultRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_group_search_result, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.owner.text = model.username
        view.groupName.text = model.group
        view.followButton.setOnClickListener {
            callback.onSubChange(position)
        }
        if (model.subscribed) {
            view.followButton.text = view.context.getString(R.string.unsub)
            view.followButton.setBackgroundColor(view.resources.getColor(R.color.error))
        } else {
            view.followButton.text = view.context.getString(R.string.sub)
            view.followButton.setBackgroundColor(view.resources.getColor(R.color.green))
        }
    }

    class Item(
        val id: Int,
        val group: String,
        val username: String,
        var subscribed: Boolean,
    )

    interface Callback {
        fun onSubChange(position: Int)
    }
}