package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.ort.studyup.common.models.Notification
import com.ort.studyup.common.models.NotificationType
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_notification.view.*

class NotificationItemRenderer(private val callback: Callback) : ViewRenderer<NotificationItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_notification, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.title.text = model.notification.title
        view.body.text = model.notification.body
        view.deleteButton.setOnClickListener {
            model.notification.id?.let {
                callback.onDeleteNotification(model.notification.id)
            }
        }
        view.setOnClickListener {
            model.notification.id?.let {
                callback.onNavigate(model.notification.id, model.notification.type, model.notification.entityId)
            }
        }
    }

    class Item(
        val notification: Notification,
    )

    interface Callback {
        fun onDeleteNotification(id: Int)
        fun onNavigate(id: Int, type: NotificationType, entityId: Int)
    }
}