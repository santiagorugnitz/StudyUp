package com.ort.studyup.home.tasks

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.renderers.DeckItemRenderer
import com.ort.studyup.common.renderers.ExamItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.game.StudyActivity
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_tasks.*

class TaskFragment : BaseFragment(), DeckItemRenderer.Callback, ExamItemRenderer.Callback {

    private val viewModel: TaskViewModel by injectViewModel(TaskViewModel::class)
    private val examAdapter = RendererAdapter()
    private val deckAdapter = RendererAdapter()

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_tasks, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        prepareList()
        initUI()
    }

    private fun prepareList() {
        examAdapter.addRenderer(ExamItemRenderer(this))
        examList.layoutManager = LinearLayoutManager(requireContext())
        examList.adapter = examAdapter
        deckAdapter.addRenderer(DeckItemRenderer(this))
        deckList.layoutManager = LinearLayoutManager(requireContext())
        deckList.adapter = deckAdapter
    }

    private fun initUI() {
        viewModel.loadItems().observe(viewLifecycleOwner, Observer {
            it?.let {
                deckAdapter.setItems(it.decks)
                examAdapter.setItems(it.exams)
            }
        })
    }

    override fun onDeckClicked(deckId: Int) {
        val intent = Intent(requireActivity(), StudyActivity::class.java)
        intent.putExtra(DECK_ID_KEY, deckId)
        startActivity(intent)
    }

    override fun onExamClicked(examId: Int) {
        Toast.makeText(requireContext(), "TODO", Toast.LENGTH_LONG).show()
    }

    override fun onAssignExam(examId: Int, groupId: Int) {
    }

}