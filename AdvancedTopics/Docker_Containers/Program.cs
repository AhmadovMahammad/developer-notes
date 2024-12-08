namespace Docker_Containers;

internal class Program
{
    private static void Main(string[] args)
    {
        /* Operating System Basics
         
        An operating system (OS) is software that manages hardware resources and 
        provides services to other software (applications). It's made up of different layers:

        1. User Space (Applicatin Layer)
        This is the layer where user applications run. 
        Programs like browsers, editors, etc., operate in this space.

        Applications in the application layer make requests to the OS 
        through system calls to interact with hardware (e.g., file reading, memory allocation).

        2. Kernel Space
        The kernel is the core part of the OS and operates at a low level, 
        directly interacting with the hardware.
        
        The kernel manages crucial tasks like memory, CPU scheduling, and device communication.
        It's responsible for resource management (CPU, memory, disk, I/O devices) and 
        ensures applications use hardware without conflicting.

        3. Hardware
        This includes all physical devices, like the CPU, RAM, storage, network interfaces, and peripherals.
        
        In simple terms, applications can't access hardware directly. 
        They request services (like writing to disk or sending data) from the kernel, which controls the hardware.


        -----What is System Calls?
        System calls are the fundamental way that application-layer programs communicate with the kernel 
        to access system resources like files, memory, devices, or network connections.

        Applications running in user space don't have direct access to hardware or kernel resources 
        (for security and stability). 
        
        When an application needs to perform low-level tasks (e.g., reading a file, allocating memory), 
        it uses system calls to request services from the kernel.

        ---How System calls work?
        
        1. Application Makes a Request: 
        The program issues a system call through an API such as the POSIX API for Unix-like systems.
        
        2. Trap to Kernel Mode: 
        The system call generates a software interrupt (trap), 
        causing the CPU to switch from user mode to kernel mode. 
        This switch allows the kernel to handle the request with full access to system resources.
        
        3. Kernel Performs the Action: 
        The kernel handles the request (e.g., reading a file from disk or allocating memory).
        
        4. Return to User Mode: 
        The kernel returns the result (or error) back to the application, 
        and the CPU switches back to user mode.

        */

        /* Virtual Machines (VMs)
         
        A virtual machine simulates a complete operating system, 
        including both the kernel and application layer. 
        It uses a hypervisor to create and run multiple VMs on one physical machine.

        -----What is Hypervisor?

        A hypervisor, also called a virtual machine monitor (VMM), 
        is specialized software that enables the creation, management, 
        and execution of virtual machines (VMs). 
        
        A hypervisor allows each virtual machine (VM) to run its own operating system and applications 
        as if it were a separate computer. 
        However, these VMs are actually sharing the physical hardware of a single host system. 
        The hypervisor manages this sharing so that each VM believes it has its own dedicated hardware.

        -----Key Roles of a Hypervisor

        1. Hardware Virtualization:
        The hypervisor abstracts and virtualizes the underlying hardware, 
        allowing each virtual machine (VM) to think it has its own dedicated resources (CPU, memory, storage, network interfaces).
        It controls how the physical hardware is allocated to each VM

        2. Isolation:
        Hypervisors isolate VMs from each other, 
        ensuring that processes in one VM don’t interfere with those in another VM.

        3. Resource Management:
        Hypervisors dynamically allocate resources like CPU time, memory, and disk space to different VMs 
        based on their needs and workloads.

        -----Types os Hypervisors?

        1. Type 1 Hypervisor Process: running directly on hardware

        NOTE: Runs directly on the physical hardware (bare-metal) without needing an underlying host operating system.
        Each VM runs its own OS, so each has its own application layer where programs run.
        Applications in the VM send requests to the VM’s OS kernel, just as they would on a physical machine.
        The VM kernel then passes these requests to the Type 1 hypervisor. 
        The hypervisor, which runs directly on the physical hardware, manages and isolates each VM’s requests.

        2. Type 2 Hypervisor Process: 

        NOTE: Runs on top of a host operating system (e.g., Windows, macOS, Linux).
        Like in Type 1, each VM runs its own OS and has its own application layer.
        Applications within the VM communicate with their VM’s kernel to request hardware resources.
        The VM kernel then passes these requests to the Type 2 hypervisor.
        Since a Type 2 hypervisor runs on top of a host OS, it doesn’t directly control hardware. 
        Instead, it sends the request to the host OS kernel, which has direct control over the physical hardware.
        The host OS kernel then accesses the physical hardware and provides resources to the VM through the hypervisor.

        ---Summary of Process Differences
        Environment	            Steps for Application to Access Hardware
        
        Normal OS	            Application → OS Kernel → Hardware: Application requests go directly to the OS kernel, which manages hardware.
        Type 1 Hypervisor	    VM Application → VM Kernel → Type 1 Hypervisor → Hardware: Hypervisor manages hardware requests for all VMs directly.
        Type 2 Hypervisor	    VM Application → VM Kernel → Type 2 Hypervisor → Host OS Kernel → Hardware: Hypervisor relies on host OS for hardware access, adding an extra layer.

        -----How Hypervisors Work:
        
        When a hypervisor creates a VM, it assigns 
        a virtual CPU, virtual memory, virtual storage, and virtual network interfaces to the VM.
        
        The VM’s OS and applications run on these virtualized components, thinking they are real hardware.

        ---Handling System Calls:
        When a VM’s operating system makes system calls to access hardware (e.g., writing to a disk), 
        the hypervisor intercepts these calls and translates them to real hardware instructions.
        The hypervisor ensures that each VM only accesses the resources assigned to it.

        */

        /* Docker and Containers


        */
    }
}