"use client";
import TaskListGrid from "@/components/TaskListGrid";

export default function Home() {

    return <div className="min-h-screen min-w-screen dark:bg-gray-700 bg-green-50 py-8">
        <div className="container max-w-[64rem] mx-auto px-6">
            <TaskListGrid/>
        </div>
    </div>
}