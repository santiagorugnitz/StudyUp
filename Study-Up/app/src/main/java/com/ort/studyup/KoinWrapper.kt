package com.ort.studyup

import android.app.Application
import androidx.room.Room
import com.ort.studyup.common.ui.ResourceWrapper
import com.ort.studyup.common.utils.EncryptedPreferencesHelper
import com.ort.studyup.study.StudyViewModel
import com.ort.studyup.home.decks.DeckDetailViewModel
import com.ort.studyup.home.decks.DecksViewModel
import com.ort.studyup.home.decks.FollowingDecksViewModel
import com.ort.studyup.home.decks.NewDeckViewModel
import com.ort.studyup.home.exams.ExamDetailViewModel
import com.ort.studyup.home.exams.ExamsViewModel
import com.ort.studyup.home.exams.NewExamViewModel
import com.ort.studyup.home.decks.flashcards.NewFlashcardViewModel
import com.ort.studyup.home.exams.examcards.NewExamCardViewModel
import com.ort.studyup.home.groups.GroupsViewModel
import com.ort.studyup.home.groups.NewGroupViewModel
import com.ort.studyup.home.profile.ProfileViewModel
import com.ort.studyup.home.profile.RankingViewModel
import com.ort.studyup.home.search.SearchViewModel
import com.ort.studyup.home.tasks.TaskViewModel
import com.ort.studyup.login.LoginViewModel
import com.ort.studyup.login.RegisterViewModel
import com.ort.studyup.repositories.*
import com.ort.studyup.services.*
import com.ort.studyup.splash.SplashViewModel
import com.ort.studyup.storage.dao.AppDatabase
import com.ort.studyup.test.TestViewModel
import org.koin.android.ext.koin.androidContext
import org.koin.android.ext.koin.androidLogger
import org.koin.core.context.startKoin
import org.koin.dsl.module

object KoinWrapper {

    fun start(application: Application) {
        startKoin {
            androidLogger()
            androidContext(application)
            modules(
                listOf(
                    utils(),
                    viewModels(),
                    repositories(),
                    services(),
                    database(),
                )
            )
        }
    }

    private fun utils() = module {
        factory { ResourceWrapper(get()) }
        factory { EncryptedPreferencesHelper(get()) }

    }

    private fun viewModels() = module {
        factory { LoginViewModel(get(), get()) }
        factory { RegisterViewModel(get(), get()) }
        factory { SplashViewModel(get()) }
        factory { DecksViewModel(get(), get()) }
        factory { DeckDetailViewModel(get(),get()) }
        factory { NewDeckViewModel(get(), get()) }
        factory { NewFlashcardViewModel(get(), get()) }
        factory { NewGroupViewModel(get(), get()) }
        factory { ProfileViewModel(get()) }
        factory { SearchViewModel(get(), get()) }
        factory { FollowingDecksViewModel(get(), get()) }
        factory { StudyViewModel(get()) }
        factory { GroupsViewModel(get(), get(), get()) }
        factory { ExamsViewModel(get(), get(), get()) }
        factory { ExamDetailViewModel(get()) }
        factory { NewExamViewModel(get(), get()) }
        factory { NewExamCardViewModel(get(), get()) }
        factory { TaskViewModel(get()) }
        factory { TestViewModel(get()) }
        factory { RankingViewModel(get()) }
    }

    private fun repositories() = module {
        factory { UserRepository(get(), get(), get()) }
        factory { DeckRepository(get()) }
        factory { FlashcardRepository(get()) }
        factory { GroupRepository(get()) }
        factory { ExamRepository(get()) }
        factory { ExamCardRepository(get()) }
        factory { TaskRepository(get()) }

    }

    private fun services() = module {
        factory { ServiceFactory(get()).createInstance(UserService::class.java) }
        factory { ServiceFactory(get()).createInstance(DeckService::class.java) }
        factory { ServiceFactory(get()).createInstance(FlashcardService::class.java) }
        factory { ServiceFactory(get()).createInstance(GroupService::class.java) }
        factory { ServiceFactory(get()).createInstance(ExamService::class.java) }
        factory { ServiceFactory(get()).createInstance(ExamCardService::class.java) }
        factory { ServiceFactory(get()).createInstance(TaskService::class.java) }
    }

    private fun database() = module {
        single {
            Room.databaseBuilder(
                androidContext(),
                AppDatabase::class.java, "studyUpDatabase"
            )
                .fallbackToDestructiveMigration()
                .build()
        }
        factory { get<AppDatabase>().userDao() }
    }

}