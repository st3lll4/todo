"use client";
import { useEffect, useState } from "react";
import { TaskListService } from "../../../../services/taskListService";
import { TaskList } from "../../../../types/TaskList";
import { ListItem } from "../../../../types/ListItem";
import { ListItemService } from "../../../../services/listItemService";
import { useParams } from "next/navigation";

export default function Home() {
  const taskListService = new TaskListService();
  const listItemService = new ListItemService();

  const [lists, setLists] = useState<TaskList[]>();
  const [items, setItems] = useState<ListItem[]>();

  const params = useParams();
  const id = params.id;

  useEffect(() => {
    const fetchData = async () => {
        console.log(id)
      const res = await taskListService.getAllAsync();
      const res2 = await listItemService.getListItemsByTaskList(id?.toString() || '');
      if (res.errors || res2.errors) {
        console.log(res.errors);
        console.log(res2.errors);
        return;
      }
      console.log("items", res2.data);
      setLists(res.data);
      setItems(res2.data);
    };
    fetchData();
  }, []);

  return (
    <main>
      <h1>Hello, World!</h1>
      <div className="lists-container">
        {lists?.map((list) => (
          <div key={list.id} className="list-item">
            {list.title}
          </div>
        ))}
        .......
        {items?.map((item) => (
          <div key={item.id} className="list-item">
            {item.description}
          </div>
        ))}
      </div>
    </main>
  );
}
