package com.ort.studyup.home.groups

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.renderers.AssignDeckItemRenderer
import com.ort.studyup.common.renderers.DeletableDeckItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_groups.*

class GroupsFragment : BaseFragment(), DeletableDeckItemRenderer.Callback, AssignDeckItemRenderer.Callback {

    private val viewModel: GroupsViewModel by injectViewModel(GroupsViewModel::class)
    private val adapter = RendererAdapter()

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_groups, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        fab.setOnClickListener { findNavController().navigate(R.id.action_groupsFragment_to_newGroupFragment) }

        adapter.addRenderer(DeletableDeckItemRenderer(this))
        adapter.addRenderer(AssignDeckItemRenderer(this))
        groupList.layoutManager = LinearLayoutManager(requireContext())
        groupList.adapter = adapter
        loadItems()
    }

    private fun loadItems(){
        viewModel.loadGroups().observe(viewLifecycleOwner, {
            adapter.setItems(it)
        })
    }

    override fun onDeleteDeck(groupId: Int, deckId: Int) {
        viewModel.unassignDeck(groupId, deckId).observe(viewLifecycleOwner, {
            loadItems()
        })
    }

    override fun onAssignDeck(groupId: Int, deckId: Int) {
        viewModel.assignDeck(groupId, deckId).observe(viewLifecycleOwner, {
            loadItems()
        })
    }
}