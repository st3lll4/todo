"use client";
import TaskListGrid from "@/components/TaskListGrid";
import Link from "next/link";

export default function Home() {

    return <div className="min-h-screen min-w-[52rem] bg-green-50 py-8">
        <div className="container mx-auto px-6">
            <TaskListGrid/>
        </div>
    </div>
}