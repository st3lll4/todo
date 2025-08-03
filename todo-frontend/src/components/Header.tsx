"use client";
import { Navbar, NavbarCollapse, NavbarLink } from "flowbite-react";
import { usePathname } from "next/navigation";

export default function Header() {
  const pathname = usePathname();
  return (
    <Navbar fluid rounded>
      <NavbarCollapse>
        <NavbarLink href="/" active={pathname === "/"}>
          Home
        </NavbarLink>
        <NavbarLink href="/lists/new" active={pathname === "/lists/new"}>
          Create
        </NavbarLink>
      </NavbarCollapse>
    </Navbar>
  );
}
