package com.ort.studyup.home.tasks

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.EXAM_ID_KEY
import com.ort.studyup.common.models.NotificationType
import com.ort.studyup.common.renderers.NotificationItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.study.StudyActivity
import com.ort.studyup.test.TestActivity
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_notifications.*

class NotificationFragment : BaseFragment(), NotificationItemRenderer.Callback {

    private val viewModel: NotificationViewModel by injectViewModel(NotificationViewModel::class)
    private val adapter = RendererAdapter()

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_notifications, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        prepareList()
        initUI()
    }

    private fun prepareList() {
        adapter.addRenderer(NotificationItemRenderer(this))
        notificationList.layoutManager = LinearLayoutManager(requireContext())
        notificationList.adapter = adapter
    }

    private fun initUI() {
        viewModel.loadItems().observe(viewLifecycleOwner, {
            it?.let {
                adapter.setItems(it)
            }
        })
        deleteAll.setOnClickListener {
            viewModel.clearNotifications().observe(viewLifecycleOwner, {
                if (it) adapter.setItems(listOf())
            })
        }
    }

    override fun onDeleteNotification(id: Int) {
        viewModel.delete(id).observe(viewLifecycleOwner, { adapter.setItems(it) })
    }

    override fun onNavigate(id: Int, type: NotificationType, entityId: Int) {
        when (type) {
            NotificationType.DECK -> {
                val intent = Intent(requireActivity(), StudyActivity::class.java)
                intent.putExtra(DECK_ID_KEY, entityId)
                startActivity(intent)
            }
            NotificationType.EXAM -> {
                val intent = Intent(requireActivity(), TestActivity::class.java)
                intent.putExtra(EXAM_ID_KEY, entityId)
                startActivity(intent)
            }
            NotificationType.COMMENT -> {
                findNavController().navigate(
                    R.id.action_notificationFragment_to_deckDetailFragment2,
                    Bundle().apply { putInt(DECK_ID_KEY, entityId) })
            }
        }
    }


}