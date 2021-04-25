package com.ort.studyup.home.decks

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.observe
import androidx.navigation.fragment.findNavController
import com.ort.studyup.R
import com.ort.studyup.common.DECK_DATA_KEY
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.common.ui.ConfirmationDialog
import kotlinx.android.synthetic.main.fragment_new_deck.*
import kotlinx.android.synthetic.main.item_spinner.view.*

class NewDeckFragment : BaseFragment(), ConfirmationDialog.Callback {

    private val viewModel: NewDeckViewModel by injectViewModel(NewDeckViewModel::class)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_new_deck, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        initSpinner(
            difficultyInput,
            resources.getStringArray(R.array.difficulties),
            getString(R.string.difficulty)
        )
        initSpinner(
            visibilityInput,
            resources.getStringArray(R.array.visibilities),
            getString(R.string.visibility)
        )

        (arguments?.getSerializable(DECK_DATA_KEY) as DeckData?)?.let { deckData ->
            viewModel.deckId = deckData.id
            header.text = getString(R.string.edit_deck)
            nameInput.setText(deckData.name)
            subjectInput.setText(deckData.subject)
            difficultyInput.spinner.setSelection(deckData.difficulty)
            if (deckData.isHidden) {
                visibilityInput.spinner.setSelection(1)
            }
            saveButton.text = getString(R.string.save_changes)
            deleteButton.visibility = View.VISIBLE
            deleteButton.setOnClickListener {
                ConfirmationDialog(
                    requireContext(),
                    getString(R.string.delete_deck_confirmation),
                    this,
                    attrs = null
                ).show()
            }
        }


        saveButton.setOnClickListener {
            viewModel.sendData(
                DeckData(
                    name = nameInput.text.toString(),
                    author = "",
                    subject = subjectInput.text.toString(),
                    difficulty = difficultyInput.spinner.selectedItemPosition,
                    isHidden = visibilityInput.spinner.selectedItemPosition == 1
                )
            ).observe(viewLifecycleOwner, {
                if (it > 0) {
                    findNavController().navigate(
                        R.id.action_newDeckFragment_to_deckDetailFragment,
                        Bundle().apply { putInt(DECK_ID_KEY, it) })
                }
            })

        }
    }

    override fun onButtonClick() {
        viewModel.deleteDeck().observe(viewLifecycleOwner, {
            if (it) {
                requireActivity().onBackPressed()
                requireActivity().onBackPressed()
                requireActivity().onBackPressed()
            }
        })
    }
}